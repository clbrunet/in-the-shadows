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
        LevelManager.OnLevelCompletion += IncreaseRange;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelCompletion -= IncreaseRange;
    }

    private IEnumerator Flash()
    {
        float elapsed = 0f;
        float duration = 1f;
        float start = light.range;
        float end = start + 10;
        while (elapsed < duration)
        {
            yield return null;
            elapsed += Time.deltaTime;
            light.range = Mathf.Lerp(start, end, elapsed / duration);
        }
        elapsed = 0f;
        while (elapsed < duration)
        {
            yield return null;
            elapsed += Time.deltaTime;
            light.range = Mathf.Lerp(end, start, elapsed / duration);
        }
    }

    private void IncreaseRange()
    {
        StartCoroutine(Flash());
    }
}
