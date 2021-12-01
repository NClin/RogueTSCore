using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

    private Camera cam;
    
    private float keyMoveSpeed;
    private float zoomSpeed;

    void Awake()
    {
        keyMoveSpeed = 0.3f;
        zoomSpeed = 0.5f;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            cam.transform.position += Vector3.up * keyMoveSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            cam.transform.position += Vector3.down * keyMoveSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            cam.transform.position += Vector3.left * keyMoveSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            cam.transform.position += Vector3.right * keyMoveSpeed;
        }

        if (Input.mouseScrollDelta != Vector2.zero)
        {


            cam.orthographicSize -= Input.mouseScrollDelta.y * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 2, 30f);

        }
    }
}
