using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Normal,
    Test,
}

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject optionsUIManager;

    public static GameMode gameMode;

    private void Start()
    {
        OptionsUIManager.OnBack += OnOptionsBack;
    }

    private void OnDestroy()
    {
        OptionsUIManager.OnBack -= OnOptionsBack;
    }

    private void Play()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void PlayNormalMode()
    {
        gameMode = GameMode.Normal;
        Play();
    }

    public void PlayTestMode()
    {
        gameMode = GameMode.Test;
        Play();
    }

    public void Options()
    {
        optionsUIManager.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnOptionsBack()
    {
        gameObject.SetActive(true);
        optionsUIManager.SetActive(false);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
