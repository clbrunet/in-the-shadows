using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        Vector2 topLeft = new();
        Vector2 bottomRight = new();
        foreach (LevelSelector levelSelector in levelSelectors.GetComponentsInChildren<LevelSelector>())
        {
            if (levelSelector.transform.position.x < topLeft.x)
            {
                topLeft.x = levelSelector.transform.position.x;
            }
            if (levelSelector.transform.position.y > topLeft.y)
            {
                topLeft.y = levelSelector.transform.position.y;
            }
            if (levelSelector.transform.position.x > bottomRight.x)
            {
                bottomRight.x = levelSelector.transform.position.x;
            }
            if (levelSelector.transform.position.y < bottomRight.y)
            {
                bottomRight.y = levelSelector.transform.position.y;
            }
        }
        camera.SetBoundaries(topLeft, bottomRight);
    }

    public void SelectLevelSelector(LevelSelector selectedLevelSelector)
    {
        float alpha;
        foreach (LevelSelector levelSelector in levelSelectors.GetComponentsInChildren<LevelSelector>())
        {
            levelSelector.isSelected = false;
            alpha = levelSelector.nameObject.GetComponent<CanvasRenderer>().GetAlpha();
            levelSelector.nameObject.GetComponent<TMP_Text>().CrossFadeAlpha(0, alpha / 4, false);
        }
        selectedLevelSelector.isSelected = true;
        alpha = selectedLevelSelector.nameObject.GetComponent<CanvasRenderer>().GetAlpha();
        selectedLevelSelector.nameObject.GetComponent<TMP_Text>().CrossFadeAlpha(1, (1 - alpha) / 4, false);

        camera.SetDragTarget(selectedLevelSelector.transform.position);
    }
}
