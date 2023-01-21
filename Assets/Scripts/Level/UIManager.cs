using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameUI;
    [SerializeField]
    private GameObject quitLevel;
    [SerializeField]
    private GameObject pauseMenu;

    private void OnEnable()
    {
        LevelManager.OnLevelCompletion += OnLevelCompletion;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelCompletion -= OnLevelCompletion;
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
        quitLevel.SetActive(false);
    }

    private void OnLevelCompletion()
    {
        quitLevel.SetActive(true);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        gameUI.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
    }

    public void QuitLevel()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
