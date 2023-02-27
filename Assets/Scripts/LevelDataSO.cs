using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/LevelData")]
public class LevelDataSO : ScriptableObject
{
    public int number;
    public string levelName;
    public bool isTutorial;
    public int pathStepCount;
    public string[] tutorialSentences;
    public Piece firstPiece;
    public Piece secondPiece;
    public Vector2[] secondPieceOffsets;
    public Vector2[] rotateAroundScales = { Vector2.one };
}
