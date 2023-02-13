using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/LevelData")]
public class LevelDataSO : ScriptableObject
{
    public int number;
    public string levelName;
    public Piece firstPiece;
    public Piece secondPiece;
    public Vector2 secondPieceOffset;
}
