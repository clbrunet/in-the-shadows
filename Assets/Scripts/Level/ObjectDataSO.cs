using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct TargetRotationsCfg
{
    public float[] xs;
    public float[] ys;
    public float[] zs;
}

[CreateAssetMenu(menuName = "ScriptableObject/ObjectData")]
public class ObjectDataSO : ScriptableObject
{
    public GameObject prefab;
    public Quaternion startRotation;

    public TargetRotationsCfg[] targetRotationsCfgs;
}
