using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    private LevelDataSO levelData;
    private Vector2 mouseDownPosition;

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
        LevelManager.levelData = levelData;
        SceneManager.LoadScene("Level");
    }
}
