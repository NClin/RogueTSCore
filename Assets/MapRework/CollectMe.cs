using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMe : MonoBehaviour
{
    private PlayerRes playerRes;

    private void Start()
    {
        playerRes = FindObjectOfType<PlayerRes>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<CollectsData>() != null)
        {
            playerRes.ChangeData(1);
            Destroy(gameObject);
        }
    }

}
