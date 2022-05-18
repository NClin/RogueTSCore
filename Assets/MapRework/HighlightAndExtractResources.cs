using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightAndExtractResources : MonoBehaviour
{
    [SerializeField]
    private Color highlightColor;
    [SerializeField]
    private float range;
    
    [SerializeField]
    private float extractionFrequency;
    [SerializeField]
    private int extractionAmount;
    [SerializeField]
    private GameObject extractionIndicator;

    [SerializeField]
    private PlayerController playerController;


    private MapState mapState;
    private List<Coroutine> extractionCoroutines;
    private List<GameObject> extractionIndicators;
    private List<ResourceNode> highlightedNodes;


    void Start()
    {
        mapState = FindObjectOfType<MapState>();
        extractionCoroutines = new List<Coroutine>();
        extractionIndicators = new List<GameObject>();
        highlightedNodes = new List<ResourceNode>();
    }

    // Update is called once per frame
    void Update()
    {

        // find nodes within range
        var nearbyNodes = mapState.resourcesMap.GetNearbyResourceNodes(transform.position, range);

        // if undeployed, highlight them and ensure no extraction coroutines are ongoing.
        if (!playerController.GetDeployed())
        {
            foreach (var coroutine in extractionCoroutines)
            {
                if (coroutine != null)
                StopCoroutine(coroutine);
            }

            foreach (var indicator in extractionIndicators)
            {
                Destroy(indicator);
            }

            foreach (ResourceNode node in nearbyNodes)
            {
                node.GetComponent<SpriteRenderer>().color = highlightColor;
                highlightedNodes.Add(node);
                node.BeingExtracted = false;
            }
        }

        // otherwise extract from each nearby node.
        else
        {
            foreach (var node in nearbyNodes)
            {
                if (!node.BeingExtracted)
                {
                    Coroutine extractionCoroutine = StartCoroutine(Extract(extractionAmount, extractionFrequency, node));
                    extractionCoroutines.Add(extractionCoroutine);

                    Vector3 posV3 = new Vector3(node.position.x, node.position.y, 0);
                    var indicator = Instantiate(extractionIndicator, posV3, Quaternion.identity);
                    extractionIndicators.Add(indicator);
                }

                node.GetComponent<SpriteRenderer>().color = Color.white;
                node.BeingExtracted = true;
            }
        }

        // ensure nodes outside of range are not highlighted.
        foreach (var node in highlightedNodes)
        {
            if (!nearbyNodes.Contains(node))
            {
                node.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

    }

    private IEnumerator Extract(int amount, float frequency, ResourceNode resourceNode)
    {
        bool extracting = true;

        while (extracting)
        {
            if (resourceNode.IsEmpty())
            {
                extracting = false;
                yield return null;
            }
            var tickAmount = resourceNode.ExtractStock(amount);

            FindObjectOfType<PlayerRes>().ChangeMass(tickAmount);


            yield return new WaitForSeconds(frequency);
        }
    }
}
