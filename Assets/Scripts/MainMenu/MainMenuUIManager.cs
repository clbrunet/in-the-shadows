using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField]
    private Slider resetProgressSlider;
    private Coroutine resetProgressCoroutine;
    private bool hasResetProgress = false;
    [SerializeField]
    private AudioMixer audioMixer;

    private void Start()
    {
        OptionsUIManager.OnBack += OnOptionsBack;
    }

    private void OnEnable()
    {
        hasResetProgress = false;
        resetProgressSlider.SetValueWithoutNotify(0f);
        resetProgressSlider.GetComponentInChildren<TMP_Text>().text = "Reset data";
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

    public IEnumerator ResetProgress()
    {
        float elapsed = 0f;
        float duration = 2f;
        float start = resetProgressSlider.minValue;
        float target = resetProgressSlider.maxValue;
        while (elapsed < duration)
        {
            yield return null;
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            resetProgressSlider.SetValueWithoutNotify(Mathf.Lerp(start, target, t));
        }
        resetProgressCoroutine = null;
        PlayerPrefs.DeleteAll();
        audioMixer.SetFloat("Volume", 0f);
        LevelSelectionManager.shouldAnimateNextLevelSelector = false;
        resetProgressSlider.GetComponentInChildren<TMP_Text>().text = "Reset done";
        hasResetProgress = true;
    }

    public void OnSliderPointerDown()
    {
        if (hasResetProgress == false)
        {
            resetProgressCoroutine = StartCoroutine(ResetProgress());
        }
    }

    private void StopResetProgress()
    {
        if (resetProgressCoroutine == null)
        {
            return;
        }
        StopCoroutine(resetProgressCoroutine);
        resetProgressCoroutine = null;
        resetProgressSlider.SetValueWithoutNotify(0f);
    }

    public void OnSliderPointerUp()
    {
        StopResetProgress();
    }

    public void OnSliderPointerExit()
    {
        StopResetProgress();
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
