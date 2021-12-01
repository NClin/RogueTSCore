using UnityEngine;
using Pathfinding;
/// <summary>
/// Makes the occupied node unwalkable as long as the unit is stationary. Broken, maybe not a good approach.
/// </summary>
[RequireComponent(typeof(AILerp))]
public class BlockOccupiedNode : MonoBehaviour
{
    
    private AILerp aiLerp;
    [SerializeField]
    private float velocityThreshold;

    bool isMoving;

    private void Start()
    {
        aiLerp = GetComponent<AILerp>();
    }

    void Update()
    {
        if (aiLerp.velocity.magnitude > velocityThreshold && !isMoving)
        {
            OnMovementStart();
        }

        if (aiLerp.velocity.magnitude <= velocityThreshold)
        {
            isMoving = false;
        }

        if (!isMoving) BlockNode();



    }

    void OnMovementStart()
    {
        UnblockNode();
    }

    void BlockNode()
    {
        var bounds = GetComponent<Collider>().bounds;
        bounds.extents = new Vector3(0.5f, 0.5f, 0.4f);
        var guo = new GraphUpdateObject(bounds);
        guo.modifyWalkability = true;
        guo.setWalkability = false;
        AstarPath.active.UpdateGraphs(guo);
    }

    void UnblockNode()
    {
        var bounds = GetComponent<Collider>().bounds;
        bounds.extents = new Vector3(0.5f, 0.5f, 0.4f);
        var guo = new GraphUpdateObject(bounds);
        guo.modifyWalkability = true;
        guo.setWalkability = true;
        AstarPath.active.UpdateGraphs(guo);
    }
}
