using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private float speed = 2f;
    private new Camera camera;
    private float mousePositionZ = 4f;
    private Vector3 dragOrigin;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mousePositionZ;
        Vector3 current = camera.ScreenToWorldPoint(mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = current;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - current;
            transform.Translate(difference);
        }
    }

    private void FixedUpdate()
    {
        return;
        if (!Input.GetMouseButton(0))
        {
            return;
        }
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        if (Mathf.Approximately(x, 0f) && Mathf.Approximately(y, 0f))
        {
            //rigidbody.drag = Mathf.Min(rigidbody.drag + 1, dragMax);
        }
        else
        {
            //rigidbody.drag = drag;
        }
        Debug.Log(y);
        rigidbody.AddForce((Vector3.left * x + Vector3.back * y) * speed, ForceMode.Acceleration);
    }
}
