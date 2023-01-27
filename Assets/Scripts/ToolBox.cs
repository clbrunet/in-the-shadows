using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ToolBox
{
    public static void SetLayerRecursively(Transform root, int layer)
    {
        foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
        {
            transform.gameObject.layer = layer;
        }
    }
}
