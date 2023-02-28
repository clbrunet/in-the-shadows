using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;
    [SerializeField]
    private AudioMixer audioMixer;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        audioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume", 0f));
    }
}
