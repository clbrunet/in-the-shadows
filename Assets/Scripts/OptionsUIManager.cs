using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUIManager : MonoBehaviour
{
    [SerializeField]
    private Toggle fullscreen;

    public static Action OnBack;

    private void Start()
    {
        fullscreen.isOn = Screen.fullScreen;
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
