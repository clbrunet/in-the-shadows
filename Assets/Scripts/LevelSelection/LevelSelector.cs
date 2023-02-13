using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    private LevelSelectionManager levelSelectionManager;
    [SerializeField]
    private LevelDataSO levelData;
    private Vector2 mouseDownPosition;
    public bool isSelected;
    public GameObject nameObject;

    private void Awake()
    {
        levelSelectionManager = FindObjectOfType<LevelSelectionManager>();
    }

    private void Start()
    {
        nameObject.GetComponent<TMP_Text>().text = levelData.levelName;
        nameObject.GetComponent<CanvasRenderer>().SetAlpha(0);
    }

    private void OnMouseDown()
    {
        mouseDownPosition = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        Vector2 delta = (Vector2)Input.mousePosition - mouseDownPosition;
        delta.x = Mathf.Abs(delta.x);
        delta.y = Mathf.Abs(delta.y);
        if (delta.x + delta.y > 10)
        {
            return;
        }
        if (isSelected == false)
        {
            levelSelectionManager.SelectLevelSelector(this);
            return;
        }
        LevelManager.levelData = levelData;
        SceneManager.LoadScene("Level");
    }
}
