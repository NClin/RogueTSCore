using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveforward : MonoBehaviour
{
    [SerializeField]
    float speed = 2;
    [SerializeField]
    float rotation = 3;

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * transform.right * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, rotation));
    }
}
