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
    private const float secondPieceOffsetMaxDifference = 0.05f;

    private const float degrees = 250f;

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
        ToolBox.SetLayerRecursively(firstPiece.transform, GetPieceMask(true));
        if (levelData.secondPiece != null)
        {
            secondPiece = Instantiate(levelData.secondPiece);
        }
    }

    private bool CheckSecondPieceOffset()
    {
        Vector2 offset = secondPiece.transform.position - firstPiece.transform.position;
        Vector2 difference = levelData.secondPieceOffset - offset;
        if (Mathf.Abs(difference.x) < secondPieceOffsetMaxDifference
            && Mathf.Abs(difference.y) < secondPieceOffsetMaxDifference)
        {
            Vector3 displacement = new Vector3(difference.x, difference.y, 0) / 2;
            firstPiece.targetPosition = firstPiece.transform.position - displacement;
            secondPiece.targetPosition = secondPiece.transform.position + displacement;
            return true;
        }
        return false;
    }

    private bool CheckCompletion()
    {
        if (isLevelCompleted)
        {
            return true;
        }
        bool firstPieceCheck = firstPiece.CheckRotation();
        bool secondPieceCheck = secondPiece == null || (secondPiece.CheckRotation() && CheckSecondPieceOffset());
        if (firstPieceCheck && secondPieceCheck)
        {
            isLevelCompleted = true;
            OnLevelCompletion?.Invoke();
            return true;
        }
        return false;
    }

    private void MovePieces()
    {
        if (!(Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(0)))
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

    private int GetPieceMask(bool isSelected)
    {
        if (isSelected)
        {
            return LayerMask.NameToLayer("Selected Piece");
        }
        else
        {
            return LayerMask.NameToLayer("Default");
        }
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
            ToolBox.SetLayerRecursively(firstPiece.transform, GetPieceMask(firstPiece.isSelected));
            secondPiece.isSelected = !secondPiece.isSelected;
            ToolBox.SetLayerRecursively(secondPiece.transform, GetPieceMask(secondPiece.isSelected));

        }
    }

    private void Update()
    {
        if (CheckCompletion())
        {
            return;
        }
        if (secondPiece != null)
        {
            MovePieces();
            CheckSelectionChange();
        }
    }
}
