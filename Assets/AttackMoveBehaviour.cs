using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class AttackMoveBehaviour : MonoBehaviour
{
    public Vector3Int destination;
    bool moving = false;

    void Start()
    {
        GetComponent<Movement>().MoveTo(destination);
        moving = true;
    }

    void Update()
    {
        if (GetComponent<HasTarget>().hasTargetTrue())
        {
            Halt();
        }

        if (!GetComponent<HasTarget>().hasTargetTrue()
            && !moving)
        {
            Resume();
        }
    }

    void Halt()
    {
        GetComponent<Movement>().StopOrder();
        moving = false;
    }

    void Resume()
    {
        GetComponent<Movement>().MoveTo(destination);
        moving = true;
    }
}
