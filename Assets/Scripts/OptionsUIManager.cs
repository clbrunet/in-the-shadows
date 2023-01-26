using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUIManager : MonoBehaviour
{
    private Resolution[] resolutions;
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private Toggle fullscreenToggle;

    public static Action OnBack;

    private void Start()
    {
        resolutions = Screen.resolutions;
        Array.Reverse(resolutions);
        List<string> options = new();
        int input = 0;
        int i = 0;
        foreach (Resolution resolution in resolutions)
        {
            options.Add(resolution.width + "x" + resolution.height);
            if (resolution.width == Screen.width && resolution.height == Screen.height)
            {
                input = i;
            }
            i++;
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.SetValueWithoutNotify(input);
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
    }

    public void OnResolutionValueChange(int value)
    {
        Screen.SetResolution(resolutions[value].width, resolutions[value].height, Screen.fullScreen);
    }

    public void OnFullscreenValueChange(bool value)
    {
        Screen.fullScreen = value;
    }

    public void Back()
    {
        OnBack?.Invoke();
    }
}
