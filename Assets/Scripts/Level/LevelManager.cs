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
    private Piece secondPiece = null;

    private const float degrees = 500f;

    private float shiftDownTime;

    private void Awake()
    {
        if (levelData == null)
        {
            levelData = defaultLevelData;
        }
    }

    private void Start()
    {
        firstPiece = Instantiate(levelData.firstPiece);
        firstPiece.isSelected = true;
        if (levelData.secondPiece != null)
        {
            secondPiece = Instantiate(levelData.secondPiece);
        }
    }

    private void CheckCompletion()
    {
        if (firstPiece.CheckRotation() && (secondPiece == null || secondPiece.CheckRotation()))
        {
            isLevelCompleted = true;
            OnLevelCompletion?.Invoke();
        }
    }

    private void MovePieces()
    {
        if (!(secondPiece != null && Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(0)))
        {
            return;
        }
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        Quaternion firstPieceRotation = firstPiece.transform.rotation;
        Quaternion secondPieceRotation = secondPiece.transform.rotation;
        firstPiece.transform.RotateAround(Vector3.zero, Vector3.down, x * degrees * Time.deltaTime);
        secondPiece.transform.RotateAround(Vector3.zero, Vector3.down, x * degrees * Time.deltaTime);
        firstPiece.transform.RotateAround(Vector3.zero, Vector3.right, y * degrees * Time.deltaTime);
        secondPiece.transform.RotateAround(Vector3.zero, Vector3.right, y * degrees * Time.deltaTime);
        firstPiece.transform.rotation = firstPieceRotation;
        secondPiece.transform.rotation = secondPieceRotation;
    }
    private void CheckSelectionChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shiftDownTime = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && Time.time - shiftDownTime < 0.5f)
        {
            firstPiece.isSelected = !firstPiece.isSelected;
            secondPiece.isSelected = !secondPiece.isSelected;
        }
    }

    private void Update()
    {
        if (isLevelCompleted)
        {
            return;
        }
        CheckCompletion();
        MovePieces();
        CheckSelectionChange();
    }
}
