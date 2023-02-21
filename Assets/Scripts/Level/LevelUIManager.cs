using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameUI;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject quitLevel;
    [SerializeField]
    private GameObject optionsUIManager;

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
        OptionsUIManager.OnBack += OnOptionsBack;
    }

    private void OnDestroy()
    {
        OptionsUIManager.OnBack -= OnOptionsBack;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
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

    public void Options()
    {
        optionsUIManager.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void OnOptionsBack()
    {
        pauseMenu.SetActive(true);
        optionsUIManager.SetActive(false);
    }

    public void QuitLevel()
    {
        SceneManager.LoadScene("LevelSelection");
        Time.timeScale = 1f;
    }
}
