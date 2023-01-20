using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private LevelManager levelManager;
    [SerializeField]
    private GameObject gameUI;
    [SerializeField]
    private GameObject quitLevel;
    [SerializeField]
    private GameObject pauseMenu;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnEnable()
    {
        levelManager.onLevelCompletion += OnLevelCompletion;
    }

    private void OnDisable()
    {
        levelManager.onLevelCompletion -= OnLevelCompletion;
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
