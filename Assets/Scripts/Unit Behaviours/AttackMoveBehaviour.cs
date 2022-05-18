using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementStripped))]
public class AttackMoveBehaviour : MonoBehaviour
{
    bool moving = false;
    MovementStripped movement;

    private Vector3Int _destination;
    public Vector3Int destination { 
        get
        {
            return _destination;
        }
        set
        {
            if (value != _destination)
            {
                movement.MoveTo(destination);
            }
            _destination = value;
        }
    }
    

    void Start()
    {
        movement = GetComponent<MovementStripped>();
        destination = Vector3Int.zero;
    }

    void Update()
    {
        if (GetComponent<HasTarget>().hasTarget())
        {
            Halt();
        }

        if (!GetComponent<HasTarget>().hasTarget())
        {
            Resume();
        }

    }

    void Halt()
    {
        movement.StopOrder();
        moving = false;
    }

    void Resume()
    {
        movement.MoveTo(destination);
        moving = true;
    }
}
