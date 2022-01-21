using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

    private Camera cam;
    
    [SerializeField]
    private float keyMoveSpeed;
    [SerializeField]
    private float zoomSpeed;
    private float zoomFactor;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = 8;
        SetZoomFactor();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            cam.transform.position += Vector3.up * keyMoveSpeed * zoomFactor * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            cam.transform.position += Vector3.down * keyMoveSpeed * zoomFactor * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            cam.transform.position += Vector3.left * keyMoveSpeed * zoomFactor * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            cam.transform.position += Vector3.right * keyMoveSpeed * zoomFactor * Time.deltaTime;
        }

        if (Input.mouseScrollDelta != Vector2.zero)
        {
            // TODO: set these as targets, and use a lerp coroutine to zoom the camera smoothly
            cam.orthographicSize -= Input.mouseScrollDelta.y * Input.mouseScrollDelta.y * Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 2, 40f);

            SetZoomFactor();
        }
    }

    private void SetZoomFactor()
    {
        zoomFactor = (cam.orthographicSize / 40);
    }
}
