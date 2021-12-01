using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnit : MonoBehaviour
{
    [SerializeField]
    private GameObject unitToSpawn;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            Instantiate(unitToSpawn, Camera.main.ScreenToWorldPoint(Input.mousePosition), unitToSpawn.transform.rotation);
        }
    }
}
