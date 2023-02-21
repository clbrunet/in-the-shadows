using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TargetRotationsCfg
{
    public float[] xs;
    public float[] ys;
    public float[] zs;
}

public class Piece : MonoBehaviour
{
    [HideInInspector]
    public bool isSelected = false;
    private new Rigidbody rigidbody;
    [SerializeField]
    private TargetRotationsCfg[] targetRotationsCfgs;
    private const float angleMaxDifference = 6f;
    [HideInInspector]
    public Vector3 targetPosition = Vector3.zero;
    [HideInInspector]
    public Quaternion targetRotation;

    [SerializeField]
    private Vector3 angularScale = Vector3.one;
    private const float angularSpeed = 5f;
    private const float angularDrag = 1f;
    private const float angularDragMax = 10f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        LevelManager.OnLevelCompletion += OnLevelCompletion;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelCompletion -= OnLevelCompletion;
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
            Match bestMatch = new(current, 360f);
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

    public bool CheckRotation()
    {
        float x = transform.eulerAngles.x;
        float y = transform.eulerAngles.y;
        float z = transform.eulerAngles.z;
        foreach (TargetRotationsCfg targetRotationsCfg in targetRotationsCfgs)
        {
            Match xBestMatch = Match.GetBest(x, targetRotationsCfg.xs);
            Match yBestMatch = Match.GetBest(y, targetRotationsCfg.ys);
            Match zBestMatch = Match.GetBest(z, targetRotationsCfg.zs);
            if ((xBestMatch.difference < angleMaxDifference && yBestMatch.difference < angleMaxDifference && zBestMatch.difference < angleMaxDifference)
                || xBestMatch.difference + yBestMatch.difference + zBestMatch.difference < angleMaxDifference * 2)
            {
                targetRotation = Quaternion.Euler(xBestMatch.target, yBestMatch.target, zBestMatch.target);
                return true;
            }
        }
        return false;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rigidbody.angularVelocity = Vector3.zero;
            return;
        }
        if (!(isSelected && Input.GetMouseButton(0)))
        {
            return;
        }
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        if (Mathf.Approximately(x, 0f) && Mathf.Approximately(y, 0f))
        {
            rigidbody.angularDrag = Mathf.Min(rigidbody.angularDrag + 1, angularDragMax);
        }
        else
        {
            rigidbody.angularDrag = angularDrag;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            rigidbody.AddTorque(Vector3.Scale(Vector3.forward * y, angularScale) * angularSpeed, ForceMode.Acceleration);
        }
        else
        {
            rigidbody.AddTorque(Vector3.Scale(Vector3.down * x + Vector3.right * y, angularScale) * angularSpeed, ForceMode.Acceleration);
        }
    }

    private IEnumerator TranslateAndRotateToTarget()
    {
        float elapsed = 0f;
        float duration = 1f;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        while (elapsed < duration)
        {
            yield return null;
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.SetPositionAndRotation(Vector3.Lerp(startPosition, targetPosition, t), Quaternion.Slerp(startRotation, targetRotation, t));
        }
    }

    private void OnLevelCompletion()
    {
        isSelected = false;
        rigidbody.angularVelocity = Vector3.zero;
        StartCoroutine(TranslateAndRotateToTarget());
    }
}
