using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPiece : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(20, 23, 12) * Time.deltaTime);
    }
}
