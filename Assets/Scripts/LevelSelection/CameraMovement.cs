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

    private Vector2 topLeftBoundary;
    private Vector2 bottomRightBoundary;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        dragTarget = transform.position;
    }

    public void MoveTo(Vector3 target)
    {
        transform.position = target;
        dragTarget = target;
    }

    public void SetBoundaries(Vector2 topLeft, Vector2 bottomRight)
    {
        topLeftBoundary = topLeft;
        topLeftBoundary.x -= 4;
        topLeftBoundary.y += 2;
        bottomRightBoundary = bottomRight;
        bottomRightBoundary.x += 4;
        bottomRightBoundary.y -= 2;
    }

    public void SetDragTarget(Vector2 value)
    {
        dragTarget.x = value.x;
        if (dragTarget.x < topLeftBoundary.x)
        {
            dragTarget.x = topLeftBoundary.x;
        }
        if (dragTarget.x > bottomRightBoundary.x)
        {
            dragTarget.x = bottomRightBoundary.x;
        }
        dragTarget.y = value.y;
        if (dragTarget.y > topLeftBoundary.y)
        {
            dragTarget.y = topLeftBoundary.y;
        }
        if (dragTarget.y < bottomRightBoundary.y)
        {
            dragTarget.y = bottomRightBoundary.y;
        }
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
            SetDragTarget(transform.position + difference);
        }
        transform.position = Vector3.SmoothDamp(transform.position, dragTarget, ref dragVelocity, dragSmoothTime);
    }
}
