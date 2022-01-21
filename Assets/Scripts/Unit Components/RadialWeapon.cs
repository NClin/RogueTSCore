using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialWeapon : MonoBehaviour
{
    [SerializeField]
    float radius;

    [SerializeField]
    float speed;

    private Rigidbody rbparent;

    private void Start()
    {
        if (transform.parent.gameObject.GetComponent<Rigidbody>() == null)
        {
            transform.parent.gameObject.AddComponent<Rigidbody>();
        }

        rbparent = transform.parent.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ElasticTransform();
    }

    private void ElasticTransform()
    {
        transform.position = transform.parent.transform.position - rbparent.velocity * 2 ;
    }
}
