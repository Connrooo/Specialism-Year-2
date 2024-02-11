using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PInputManager PInputManager;
    CameraScript CameraScript;
    PlayerMotion PlayerMotion;
    InteractScript InteractScript;

    private void Awake()
    {
        PInputManager = GetComponent<PInputManager>();
        CameraScript = FindObjectOfType<CameraScript>();
        PlayerMotion = GetComponent<PlayerMotion>();
        InteractScript = GetComponent<InteractScript>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        PlayerMotion.MovementHandler();
    }

    private void FixedUpdate()
    {
        PInputManager.InputHandler();
        InteractScript.InteractHandler();
    }

    private void LateUpdate()
    {
    }
}
