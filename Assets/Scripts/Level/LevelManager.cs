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
    private Vector2 secondPieceOffsetDifference;

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
        if (levelData.secondPiece != null)
        {
            secondPiece = Instantiate(levelData.secondPiece);
        }
    }

    private bool CheckSecondPieceOffset()
    {
        if (secondPiece == null)
        {
            return false;
        }
        Vector2 offset = secondPiece.transform.position - firstPiece.transform.position;
        Vector2 difference = levelData.secondPieceOffset - offset;
        if (Mathf.Abs(difference.x) < secondPieceOffsetMaxDifference
            && Mathf.Abs(difference.y) < secondPieceOffsetMaxDifference)
        {
            secondPieceOffsetDifference = difference;
            return true;
        }
        return false;
    }

    private IEnumerator MoveToTarget()
    {
        float elapsed = 0f;
        float duration = 1f;
        Vector3 diffToAdd = new Vector3(secondPieceOffsetDifference.x, secondPieceOffsetDifference.y, 0) / 2;
        Vector3 firstStart = firstPiece.transform.position;
        Vector3 firstEnd = firstStart - diffToAdd;
        Vector3 secondStart = secondPiece.transform.position;
        Vector3 secondEnd = secondStart + diffToAdd;
        while (elapsed < duration)
        {
            yield return null;
            elapsed += Time.deltaTime;
            firstPiece.transform.position = Vector3.Lerp(firstStart, firstEnd, elapsed / duration);
            secondPiece.transform.position = Vector3.Lerp(secondStart, secondEnd, elapsed / duration);
        }
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
            if (secondPiece != null)
            {
                StartCoroutine(MoveToTarget());
            }
            return true;
        }
        return false;
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
        if (CheckCompletion())
        {
            return;
        }
        MovePieces();
        CheckSelectionChange();
    }
}
