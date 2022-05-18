using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
[RequireComponent(typeof(Unit))]
public class Extractor : MonoBehaviour
{
    private ResourceNode? extractionNode;
    private Vector2Int myTile;

    [SerializeField]
    private int amount = 1;
    [SerializeField]
    private float frequency = 1;
    [SerializeField]
    private bool active;

    private bool extracting;


    private void Start()
    {
        myTile = VectorTools.GetClosestTileCoordinatesV2Int(transform.position);
        extractionNode = FindObjectOfType<MapState>().resourcesMap.GetNodeAt(myTile);
        if (extractionNode == null)
        {
            NodeEmpty();
        }
        else
        {
            BeginExtracting();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && GetComponent<SelectableUnit>().selected)
        {
            Debug.Log("toggling extracting");
            ToggleExtracting();
        }
    }

    public void ToggleExtracting()
    {
        if (extracting) StopExtracting();
        else BeginExtracting();
    }

    public void StopExtracting()
    {
        extracting = false;
    }

    public void BeginExtracting()
    {
        if (extractionNode != null)
        {
            if (!extracting)
            {
                StartCoroutine(Extract(amount, frequency));
            }
        }
    }

    private IEnumerator Extract(int amount, float frequency)
    {
        extracting = true;

        while(extracting)
        {
            if (extractionNode.IsEmpty())
            {
                NodeEmpty();
                extracting = false;
                yield return null;
            }
            var tickAmount = extractionNode.ExtractStock(amount);

            if (GetComponent<Unit>().team == Team.white)
            {
                FindObjectOfType<PlayerRes>().ChangeMass(tickAmount);
            }

            yield return new WaitForSeconds(frequency);
        }
    }

    private void NodeEmpty()
    {
        // A warning? A color change?
    }
}
