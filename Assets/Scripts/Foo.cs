using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foo : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("awake");
    }

    private void Start()
    {
        Debug.Log("start");
        Debug.Log(transform.eulerAngles);
        Debug.Log(transform.localEulerAngles);
    }
}
