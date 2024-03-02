using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Camera Camera;

    private void Awake()
    {
        Camera = Camera.main;
    }
    void Update()
    {
        transform.LookAt(Camera.transform);
    }
}
