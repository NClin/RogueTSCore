using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementStripped))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Sprite deployedSprite;
    [SerializeField]
    Sprite undeployedSprite;
    [SerializeField]
    float deployToggleTime = 1.5f;

    private bool initialized;
    bool deployed;

    private SpriteRenderer sr;
    private MovementStripped movementStripped;
    private MapState mapstate;

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
        sr = GetComponent<SpriteRenderer>();
        movementStripped = GetComponent<MovementStripped>();
        mapstate = GameObject.FindObjectOfType<MapState>();

        if (AstarPath.active == null) return;

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
        if (!initialized) Initialize();

        if (Input.GetKeyDown(KeyCode.Space)) 
            StartCoroutine(ToggleDeployment());

        if (!deployed) MovementInput();
    }

    private IEnumerator ToggleDeployment()
    {
        Debug.Log("toggle deploy");
        yield return new WaitForSeconds(deployToggleTime);
        if (deployed)
        {
            deployed = false;
            sr.sprite = undeployedSprite;
            Debug.Log("Deployed = " + deployed);
        }
        else
        {
            deployed = true;
            sr.sprite = deployedSprite;
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
