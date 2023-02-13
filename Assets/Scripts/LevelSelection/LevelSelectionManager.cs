using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject levelSelectors;
    [SerializeField]
    private new CameraMovement camera;

    private void Start()
    {
        int nextLevel = PlayerPrefs.GetInt("Next Level", 1);
        int level = 1;
        foreach (LevelSelector levelSelector in levelSelectors.GetComponentsInChildren<LevelSelector>(true))
        {
            if (level < nextLevel || MainMenuUIManager.gameMode == GameMode.Test)
            {
                levelSelector.gameObject.SetActive(true);
            }
            else if (level == nextLevel)
            {
                levelSelector.gameObject.SetActive(true);
                Vector3 position = levelSelector.transform.position;
                position.z = camera.transform.position.z;
                camera.MoveTo(position);
            }
            else
            {
                levelSelector.gameObject.SetActive(false);
            }
            level++;
        }
    }
}
