using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foo : MonoBehaviour
{
    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("hi");
            rb.MoveRotation(Quaternion.Euler(90, 90, 90));
        }
    }
}
