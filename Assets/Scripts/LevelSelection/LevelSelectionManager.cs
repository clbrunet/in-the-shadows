using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject levelSelectors;

    private void Start()
    {
        int nextLevel = PlayerPrefs.GetInt("Next Level", 1);
        int level = 1;
        foreach (LevelSelector levelSelector in levelSelectors.GetComponentsInChildren<LevelSelector>(true))
        {
            if (level <= nextLevel)
            {
                levelSelector.gameObject.SetActive(true);
            }
            else
            {
                levelSelector.gameObject.SetActive(false);
            }
            level++;
        }
    }
}
