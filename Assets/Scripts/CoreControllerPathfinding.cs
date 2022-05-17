using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementStripped))]
public class CoreControllerPathfinding : MonoBehaviour
{
    [SerializeField]
    float deployToggleTime = 2.5f;

    private bool initialized;
    bool deployed;

    private MovementStripped movementStripped;


    void Start()
    {
        Initialize();
    }

    void MoveFromUnwalkableTile()
    {
        transform.position = transform.position + Vector3.up;


        if (!AstarPath.active.GetNearest(transform.position).node.Walkable)
        {
            while (!AstarPath.active.GetNearest(transform.position).node.Walkable)
            {
                transform.position = transform.position + Vector3.up;
                Debug.Log("moved from unwalkable");
            }
            
            movementStripped.MoveTo(transform.position);
        }
    }

    private void Initialize()
    {

        movementStripped = GetComponent<MovementStripped>();

        if (AstarPath.active == null)
        {
            return;
        }

        if (AstarPath.active.GetNearest(transform.position).node != null)
        {
            MoveFromUnwalkableTile();
            initialized = true;
            return;
        }
        else
        {
            initialized = false;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            Initialize();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ToggleDeployment());
        }

        if (!deployed)
        {
            MovementInput();
        }
    }

    private IEnumerator ToggleDeployment()
    {
        Debug.Log("toggle deploy");
        yield return new WaitForSeconds(deployToggleTime);
        if (deployed)
        {
            deployed = false;
            Debug.Log("Deployed = " + deployed);
        }
        else
        {
            deployed = true;
            Debug.Log("Deployed = " + deployed);
        }
    }

    private void MovementInput()
    {
        if (movementStripped == null)
        {
            Debug.Log("movement not found");
            movementStripped = GetComponent<MovementStripped>();
            return;
        }

        Vector3 currentTile = VectorTools.GetClosestTileCoordinatesV3(transform.position);

        if (Input.GetKeyDown(KeyCode.W))
        {
            movementStripped.MoveTo(VectorTools.GetClosestTileCoordinatesV3(transform.position + Vector3.up));
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementStripped.MoveTo(VectorTools.GetClosestTileCoordinatesV3(transform.position + Vector3.down));
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementStripped.MoveTo(VectorTools.GetClosestTileCoordinatesV3(transform.position + Vector3.left));
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementStripped.MoveTo(VectorTools.GetClosestTileCoordinatesV3(transform.position + Vector3.right));
        }
    }

    public bool GetDeployed()
    {
        return deployed;
    }
}
