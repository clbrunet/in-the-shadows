using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyBinds
{
    public static KeyCode ForwardRotation {
        get
        {
            return (KeyCode)PlayerPrefs.GetInt("Forward Rotation", (int)KeyCode.LeftControl);
        }
        set
        {
            PlayerPrefs.SetInt("Forward Rotation", (int)value);
        }
    }

    public static KeyCode SwitchMovePieces {
        get
        {
            return (KeyCode)PlayerPrefs.GetInt("Switch Move Pieces", (int)KeyCode.LeftShift);
        }
        set
        {
            PlayerPrefs.SetInt("Switch Move Pieces", (int)value);
        }
    }
}
