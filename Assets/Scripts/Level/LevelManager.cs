using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelDataSO defaultLevelData;
    public static LevelDataSO levelData;
    private bool isLevelCompleted = false;
    public static Action OnLevelCompletion;

    private Piece firstPiece;
    private Piece secondPiece;

    void Awake()
    {
        if (levelData == null)
        {
            levelData = defaultLevelData;
        }
    }

    void Start()
    {
        firstPiece = Instantiate(levelData.firstPiece);
        firstPiece.isSelected = true;
        if (levelData.secondPiece != null)
        {
            secondPiece = Instantiate(levelData.secondPiece);
        }
    }

    void Update()
    {
        if (isLevelCompleted)
        {
            return;
        }
        if (firstPiece.CheckRotation() && (secondPiece == null || secondPiece.CheckRotation()))
        {
            isLevelCompleted = true;
            OnLevelCompletion?.Invoke();
        }
    }
}
