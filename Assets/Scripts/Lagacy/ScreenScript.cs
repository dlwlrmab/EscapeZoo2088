using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScript : MonoBehaviour
{
    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(720, 1280, false);
    }
}
