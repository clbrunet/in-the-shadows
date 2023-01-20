using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/LevelData")]
public class LevelDataSO : ScriptableObject
{
    public int number;
    public ObjectDataSO firstObjectData;
    public ObjectDataSO secondObjectData;
    public Vector2 secondObjectOffset;
}
