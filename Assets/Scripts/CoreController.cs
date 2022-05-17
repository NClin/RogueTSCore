using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreController : MonoBehaviour
{
    [SerializeField]
    float deployToggleTime = 2.5f;
    [SerializeField]
    float moveForce = 50;
    

    bool deployed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        {
           if (rb == null)
            {
                Debug.LogError("Rigidbody not found");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ToggleDeployment());
        }
    }

    private void FixedUpdate()
    {
        if (!deployed)
        {
            MovementInput();

        }
    }

    private IEnumerator ToggleDeployment()
    {
        Debug.Log("toggle deploy");
        yield return new WaitForSeconds(deployToggleTime);
        if (deployed)
        {
            deployed = false;
            Debug.Log("Deployed = " + deployed);
        }
        else
        {
            deployed = true;
            Debug.Log("Deployed = " + deployed);
        }
    }

    private void MovementInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.up * moveForce);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector3.down * moveForce);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector3.left * moveForce);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right * moveForce);
        }
    }

    public bool GetDeployed()
    {
        return deployed;
    }
}
