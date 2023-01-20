using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    private LevelDataSO levelData;

    private void OnMouseDown()
    {
        LevelManager.levelData = levelData;
        SceneManager.LoadScene("Level");
    }
}
