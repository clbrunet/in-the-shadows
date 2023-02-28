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
    /// <summary>
    /// If the piece is a tutorial piece, the array describe the path of the tutorial
    /// </summary>
    [SerializeField]
    private TargetRotationsCfg[] targetRotationsCfgs;
    private const float angleMaxDifference = 12f;
    [HideInInspector]
    public Vector3 targetPosition = Vector3.zero;
    [HideInInspector]
    public Quaternion targetRotation;

    private float x = 0f;
    private float y = 0f;
    /// <summary>
    /// If the piece is a tutorial piece, the array describe the path of the tutorial
    /// </summary>
    [SerializeField]
    private Vector3[] angularScales = { Vector3.one };
    private const float angularSpeed = 10f;
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

    public bool CheckTargetRotationsCfg(TargetRotationsCfg targetRotationsCfg)
    {
        Match xBestMatch = Match.GetBest(transform.eulerAngles.x, targetRotationsCfg.xs);
        Match yBestMatch = Match.GetBest(transform.eulerAngles.y, targetRotationsCfg.ys);
        Match zBestMatch = Match.GetBest(transform.eulerAngles.z, targetRotationsCfg.zs);
        if ((xBestMatch.difference < angleMaxDifference && yBestMatch.difference < angleMaxDifference && zBestMatch.difference < angleMaxDifference)
            || xBestMatch.difference + yBestMatch.difference + zBestMatch.difference < angleMaxDifference * 2)
        {
            targetRotation = Quaternion.Euler(xBestMatch.target, yBestMatch.target, zBestMatch.target);
            return true;
        }
        return false;
    }

    public bool CheckRotation()
    {
        if (LevelManager.levelData.isTutorial)
        {
            return CheckTargetRotationsCfg(targetRotationsCfgs[LevelManager.pathIndex]);
        }
        foreach (TargetRotationsCfg targetRotationsCfg in targetRotationsCfgs)
        {
            if (CheckTargetRotationsCfg(targetRotationsCfg))
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyBinds.SwitchMovePieces) || !isSelected || !Input.GetMouseButton(0) || Time.timeScale == 0f)
        {
            return;
        }
        x += Input.GetAxisRaw("Mouse X");
        y += Input.GetAxisRaw("Mouse Y");
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyBinds.SwitchMovePieces))
        {
            rigidbody.angularVelocity = Vector3.zero;
            return;
        }
        if (!(isSelected && Input.GetMouseButton(0)))
        {
            return;
        }
        if (Mathf.Approximately(x, 0f) && Mathf.Approximately(y, 0f))
        {
            rigidbody.angularDrag = Mathf.Min(rigidbody.angularDrag + 1, angularDragMax);
        }
        else
        {
            rigidbody.angularDrag = angularDrag;
        }

        if (Input.GetKey(KeyBinds.ForwardRotation))
        {
            rigidbody.AddTorque(Vector3.Scale(Vector3.forward * y, angularScales[LevelManager.pathIndex]) * angularSpeed);
        }
        else
        {
            rigidbody.AddTorque(Vector3.Scale(Vector3.down * x + Vector3.right * y, angularScales[LevelManager.pathIndex]) * angularSpeed);
        }
        x = 0f;
        y = 0f;
    }

    private IEnumerator TranslateAndRotateToTarget(float duration)
    {
        float elapsed = 0f;
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
        StartCoroutine(TranslateAndRotateToTarget(1f));
    }

    public void OnPathStepCompletion()
    {
        rigidbody.angularVelocity = Vector3.zero;
        StartCoroutine(TranslateAndRotateToTarget(0.2f));
    }
}
