using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackMoveBehaviour))]
public class AttackPlayer : MonoBehaviour
{
    GameObject playerEntity;
    AttackMoveBehaviour attackMoveBehaviour;

    void Start()
    {
        // clumsy, brittle, temporary.
        playerEntity = GameObject.Find("PlayerEntity");
        attackMoveBehaviour = GetComponent<AttackMoveBehaviour>();
    }

    void Update()
    {
        int x = (int)(playerEntity.transform.position.x);
        int y = (int)(playerEntity.transform.position.y);
        int z = (int)(playerEntity.transform.position.z);
        Vector3Int playerPositionV3 = new Vector3Int(x, y, z);

        attackMoveBehaviour.destination = playerPositionV3;
    }
}
