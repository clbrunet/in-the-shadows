using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLight : MonoBehaviour
{
    private LevelManager levelManager;
    private new Light light;

    private void OnEnable()
    {
        levelManager.OnLevelCompletion += IncreaseRange;
    }

    private void OnDisable()
    {
        levelManager.OnLevelCompletion -= IncreaseRange;
    }

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        light = GetComponent<Light>();
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
