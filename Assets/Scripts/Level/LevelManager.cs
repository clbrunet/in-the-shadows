using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelDataSO defaultLevelData;
    public static LevelDataSO levelData;
    [SerializeField]
    private TMP_Text levelNameText;
    [SerializeField]
    private TMP_Text levelTutorialText;
    private bool isLevelCompleted = false;
    public static Action OnLevelCompletion;

    private Piece firstPiece;
    private Piece secondPiece = null;
    private const float secondPieceOffsetMaxDifference = 0.1f;
    public static int pathIndex;

    private const float rotateAroundSpeed = 1f;

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
        levelNameText.text = levelData.levelName;

        firstPiece = Instantiate(levelData.firstPiece);
        firstPiece.isSelected = true;
        ToolBox.SetLayerRecursively(firstPiece.transform, GetPieceMask(true));
        if (levelData.secondPiece != null)
        {
            secondPiece = Instantiate(levelData.secondPiece);
        }
        pathIndex = 0;
        if (levelData.isTutorial)
        {
            levelTutorialText.text = levelData.tutorialSentences[pathIndex]
                .Replace("{ForwardRotation}", KeyBinds.ForwardRotation.ToString())
                .Replace("{SwitchMovePieces}", KeyBinds.SwitchMovePieces.ToString());
        }
    }

    private bool CheckSecondPieceOffset()
    {
        Quaternion firstPieceRotation = firstPiece.transform.rotation;
        firstPiece.transform.rotation = firstPiece.targetRotation;
        Vector2 offset = firstPiece.transform.InverseTransformPoint(secondPiece.transform.position);
        firstPiece.transform.rotation = firstPieceRotation;
        Vector2 difference = levelData.secondPieceOffsets[pathIndex] - offset;
        if (Mathf.Abs(difference.x) < secondPieceOffsetMaxDifference
            && Mathf.Abs(difference.y) < secondPieceOffsetMaxDifference)
        {
            Vector3 displacement = new(difference.x / 2, difference.y / 2, 0);
            bool x_reversed = 90 < firstPiece.transform.eulerAngles.x && firstPiece.transform.eulerAngles.x < 270;
            bool y_reversed = 90 < firstPiece.transform.eulerAngles.y && firstPiece.transform.eulerAngles.y < 270;
            bool z_reversed = 90 < firstPiece.transform.eulerAngles.z && firstPiece.transform.eulerAngles.z < 270;
            if (y_reversed != z_reversed)
            {
                displacement.x = -displacement.x;
            }
            if (x_reversed != z_reversed)
            {
                displacement.y = -displacement.y;
            }
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
        if (firstPiece.CheckRotation() &&
            (secondPiece == null || (secondPiece.CheckRotation() && CheckSecondPieceOffset())))
        {
            if (levelData.isTutorial)
            {
                pathIndex++;
                levelTutorialText.text = levelData.tutorialSentences[pathIndex]
                    .Replace("{ForwardRotation}", KeyBinds.ForwardRotation.ToString())
                    .Replace("{SwitchMovePieces}", KeyBinds.SwitchMovePieces.ToString());
                if (pathIndex < levelData.pathStepCount)
                {
                    firstPiece.OnPathStepCompletion();
                    secondPiece?.OnPathStepCompletion();
                    return false;
                }
            }
            else
            {
              levelTutorialText.text = "Congratulations !";
            }
            isLevelCompleted = true;
            if (MainMenuUIManager.gameMode == GameMode.Normal)
            {
                int nextLevel = PlayerPrefs.GetInt("Next Level", 0);
                if (levelData.number == nextLevel)
                {
                    PlayerPrefs.SetInt("Next Level", nextLevel + 1);
                    LevelSelectionManager.shouldAnimateNextLevelSelector = true;
                }
            }
            GetComponent<AudioSource>().Play();
            OnLevelCompletion?.Invoke();
            return true;
        }
        return false;
    }

    private void MovePieces()
    {
        if (!(Input.GetKey(KeyBinds.SwitchMovePieces) && Input.GetMouseButton(0)))
        {
            return;
        }
        float x = Input.GetAxisRaw("Mouse X") * rotateAroundSpeed * levelData.rotateAroundScales[pathIndex].x;
        float y = Input.GetAxisRaw("Mouse Y") * rotateAroundSpeed * levelData.rotateAroundScales[pathIndex].y;
        Quaternion firstPieceRotation = firstPiece.transform.rotation;
        Quaternion secondPieceRotation = secondPiece.transform.rotation;
        firstPiece.transform.RotateAround(Vector3.zero, Vector3.down, x);
        secondPiece.transform.RotateAround(Vector3.zero, Vector3.down, x);
        firstPiece.transform.RotateAround(Vector3.zero, Vector3.right, y);
        secondPiece.transform.RotateAround(Vector3.zero, Vector3.right, y);
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
        if (!levelData.canSwitchBetweenPieces[pathIndex])
        {
            return;
        }
        if (Input.GetKeyDown(KeyBinds.SwitchMovePieces))
        {
            shiftDownTime = Time.time;
        }
        if (Input.GetKeyUp(KeyBinds.SwitchMovePieces) && Time.time - shiftDownTime < 0.5f)
        {
            firstPiece.isSelected = !firstPiece.isSelected;
            ToolBox.SetLayerRecursively(firstPiece.transform, GetPieceMask(firstPiece.isSelected));
            secondPiece.isSelected = !secondPiece.isSelected;
            ToolBox.SetLayerRecursively(secondPiece.transform, GetPieceMask(secondPiece.isSelected));

        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }
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
