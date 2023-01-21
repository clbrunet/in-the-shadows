using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private new Camera camera;
    private const float mousePositionZ = 4f;
    private Vector3 dragOrigin;
    private Vector3 dragTarget;
    private Vector3 dragVelocity = Vector3.zero;
    private const float dragSmoothTime = 0.1f;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        dragTarget = transform.position;
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
            dragTarget = transform.position + difference;
        }
        transform.position = Vector3.SmoothDamp(transform.position, dragTarget, ref dragVelocity, dragSmoothTime);
    }
}
