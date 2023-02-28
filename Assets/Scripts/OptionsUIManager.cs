using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsUIManager : MonoBehaviour
{
    private Resolution[] resolutions;
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private Toggle fullscreenToggle;

    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private TMP_Text forwardRotationBind;
    [SerializeField]
    private TMP_Text switchMovePiecesBind;
    [SerializeField]
    private GameObject keybindPanel;
    private Action<KeyCode> keybindAction;

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

    private void OnEnable()
    {
        float volume;
        audioMixer.GetFloat("Volume", out volume);
        volumeSlider.SetValueWithoutNotify(Mathf.Pow(10, volume / 20));

        forwardRotationBind.text = KeyBinds.ForwardRotation.ToString();
        switchMovePiecesBind.text = KeyBinds.SwitchMovePieces.ToString();
        ColorKeybindsText();
    }

    public void OnResolutionValueChange(int value)
    {
        Screen.SetResolution(resolutions[value].width, resolutions[value].height, Screen.fullScreen);
    }

    public void OnFullscreenValueChange(bool value)
    {
        Screen.fullScreen = value;
    }

    public void OnVolumeValueChange(float value)
    {
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void OnForwardRotationButtonClick()
    {
        keybindPanel.SetActive(true);
        keybindAction = OnForwardRotationKeybind;
    }

    public void OnSwitchMovePiecesButtonClick()
    {
        keybindPanel.SetActive(true);
        keybindAction = OnSwitchMovePiecesKeybind;
    }

    private void ColorKeybindsText()
    {
        Color color;
        if (forwardRotationBind.text == switchMovePiecesBind.text)
        {
            color = new(1, 0, 0);
        }
        else
        {
            color = new(0, 0, 0);
        }
        forwardRotationBind.color = color;
        switchMovePiecesBind.color = color;
    }

    private void OnForwardRotationKeybind(KeyCode keyCode)
    {
        KeyBinds.ForwardRotation = keyCode;
        forwardRotationBind.text = keyCode.ToString();
        ColorKeybindsText();
    }

    private void OnSwitchMovePiecesKeybind(KeyCode keyCode)
    {
        KeyBinds.SwitchMovePieces = keyCode;
        switchMovePiecesBind.text = keyCode.ToString();
        ColorKeybindsText();
    }

    private void OnGUI()
    {
        if (!keybindPanel.activeInHierarchy)
        {
            return;
        }
        KeyCode keyCode = Event.current.keyCode;
        if (keyCode != KeyCode.None)
        {
            if (keyCode != KeyCode.Escape)
            {
                keybindAction(keyCode);
            }
            keybindPanel.SetActive(false);
        }
    }

    public void Back()
    {
        OnBack?.Invoke();
    }
}
