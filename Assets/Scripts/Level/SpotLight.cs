using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLight : MonoBehaviour
{
    private new Light light;

    private void Awake()
    {
        light = GetComponent<Light>();
    }

    private void OnEnable()
    {
        LevelManager.OnLevelCompletion += OnLevelCompletion;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelCompletion -= OnLevelCompletion;
    }

    private IEnumerator Flash()
    {
        float elapsed = 0f;
        float duration = 1f;
        float start = light.intensity;
        float end = start * 1.2f;
        while (elapsed < duration)
        {
            yield return null;
            elapsed += Time.deltaTime;
            light.intensity = Mathf.Lerp(start, end, elapsed / duration);
        }
        elapsed = 0f;
        while (elapsed < duration)
        {
            yield return null;
            elapsed += Time.deltaTime;
            light.intensity = Mathf.Lerp(end, start, elapsed / duration);
        }
    }

    private void OnLevelCompletion()
    {
        StartCoroutine(Flash());
    }
}
