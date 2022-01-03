using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

    private Camera cam;
    
    private float keyMoveSpeed;
    private float zoomSpeed;
    private float zoomFactor;

    void Awake()
    {
        keyMoveSpeed = 0.1f;
        zoomSpeed = 0.5f;
        cam = GetComponent<Camera>();
        cam.orthographicSize = 5;
        SetZoomFactor();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            cam.transform.position += Vector3.up * keyMoveSpeed * zoomFactor;
        }

        if (Input.GetKey(KeyCode.S))
        {
            cam.transform.position += Vector3.down * keyMoveSpeed * zoomFactor;
        }

        if (Input.GetKey(KeyCode.A))
        {
            cam.transform.position += Vector3.left * keyMoveSpeed * zoomFactor;
        }

        if (Input.GetKey(KeyCode.D))
        {
            cam.transform.position += Vector3.right * keyMoveSpeed * zoomFactor;
        }

        if (Input.mouseScrollDelta != Vector2.zero)
        {
            // TODO: set these as targets, and use a lerp coroutine to zoom the camera smoothly
            cam.orthographicSize -= Input.mouseScrollDelta.y * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 2, 20f);

            SetZoomFactor();
        }
    }

    private void SetZoomFactor()
    {
        zoomFactor = (cam.orthographicSize / 40);
    }
}
