using UnityEngine;
using System.Collections;

public class CameraFocal : MonoBehaviour
{
    public static CameraFocal instance;

    void Awake()
    {
        instance = this;
    }
}
