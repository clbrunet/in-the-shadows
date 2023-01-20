using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelDataSO defaultLevelData;
    public static LevelDataSO levelData;
    private bool isLevelCompleted = false;
    public event Action OnLevelCompletion;

    private GameObject firstObject;
    private GameObject secondObject = null;

    private const float angularSpeed = 30f;
    private const float angularDrag = 1f;
    private const float angularDragMax = 10f;
    private Rigidbody selectedRigidbody;

    void Awake()
    {
        if (levelData == null)
        {
            levelData = defaultLevelData;
        }
    }

    void Start()
    {
        ObjectDataSO firstObjectData = levelData.firstObjectData;
        ObjectDataSO secondObjectData = levelData.secondObjectData;
        Vector3 position = Vector3.zero;
        if (secondObjectData != null)
        {
            position.x = -1;
        }
        firstObject = Instantiate(firstObjectData.prefab, position, firstObjectData.startRotation);
        if (secondObjectData != null)
        {
            secondObject = Instantiate(secondObjectData.prefab, -position, secondObjectData.startRotation);
        }
        selectedRigidbody = firstObject.GetComponent<Rigidbody>();
    }

    private struct Match
    {
        public float target;
        public float difference;

        public Match(float target, float difference)
        {
            this.target = target;
            this.difference = difference;
        }

        public static Match GetBest(float current, float[] targets)
        {
            if (targets.Length == 0)
            {
                return new Match(current, 0f);
            }
            Match bestMatch = new Match(current, 360f);
            foreach (float target in targets)
            {
                float difference = Mathf.Abs(Mathf.DeltaAngle(current, target));
                if (difference < bestMatch.difference)
                {
                    bestMatch = new Match(target, difference);
                }
            }
            return bestMatch;
        }
    }

    private IEnumerator RotateToTarget(Quaternion targetRotation)
    {
        float elapsed = 0f;
        float duration = 1f;
        Quaternion completionRotation = firstObject.transform.rotation;
        while (elapsed < duration)
        {
            yield return null;
            elapsed += Time.deltaTime;
            firstObject.transform.rotation = Quaternion.Slerp(completionRotation, targetRotation, elapsed / duration);
        }
    }

    private void CheckObjects()
    {
        float x = firstObject.transform.eulerAngles.x;
        float y = firstObject.transform.eulerAngles.y;
        float z = firstObject.transform.eulerAngles.z;
        foreach (TargetRotationsCfg targetRotationsCfg in levelData.firstObjectData.targetRotationsCfgs)
        {
            Match xBestMatch = Match.GetBest(x, targetRotationsCfg.xs);
            Match yBestMatch = Match.GetBest(y, targetRotationsCfg.ys);
            Match zBestMatch = Match.GetBest(z, targetRotationsCfg.zs);
            if (xBestMatch.difference < 5f && yBestMatch.difference < 5f && zBestMatch.difference < 5f)
            {
                isLevelCompleted = true;
                StartCoroutine(RotateToTarget(Quaternion.Euler(xBestMatch.target, yBestMatch.target, zBestMatch.target)));
                OnLevelCompletion?.Invoke();
                return;
            }
        }
    }

    void Update()
    {
        if (isLevelCompleted)
        {
            return;
        }
        CheckObjects();
    }

    void FixedUpdate()
    {
        if (isLevelCompleted || !Input.GetMouseButton(0))
        {
            return;
        }
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        if (Mathf.Approximately(x, 0f) && Mathf.Approximately(y, 0f))
        {
            selectedRigidbody.angularDrag = Mathf.Min(selectedRigidbody.angularDrag + 1, angularDragMax);
        }
        else
        {
            selectedRigidbody.angularDrag = angularDrag;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            selectedRigidbody.AddTorque((Vector3.forward * y) * angularSpeed, ForceMode.Acceleration);
        }
        else
        {
            selectedRigidbody.AddTorque((Vector3.down * x + Vector3.right * y) * angularSpeed, ForceMode.Acceleration);
        }
    }
}
