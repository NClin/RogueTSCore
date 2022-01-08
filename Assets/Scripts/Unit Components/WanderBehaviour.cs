using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehaviour : MonoBehaviour
{

    private Movement movement;
    [SerializeField]
    private float cooldown = 3;

    float t = 0;
    int attempts = 0;
    int maxAttempts = 3;

    void Start()
    {
        movement = GetComponentInParent<Movement>();
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t > cooldown)
        {
            attempts = 0;
            Wander();
            t = 0;
        }
    }

    private void Wander()
    {
        
        var dir = Random.insideUnitCircle.normalized;
        var dest = transform.position + new Vector3(dir.x, dir.y, 0);

        
        if (!AstarPath.active.GetNearest(dest).node.Walkable 
            && attempts <= maxAttempts) 
        {
            attempts++;
            Wander();
        }
        else
        {
            movement.MoveTo(dest);
        }
        
    }
}
