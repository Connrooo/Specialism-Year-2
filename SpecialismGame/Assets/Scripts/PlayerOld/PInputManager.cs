using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PInputManager : MonoBehaviour
{
    [Header("Input Start Up")]
    ISInputSystem PlayerInput;

    [Header("Movement Controls")]
    [SerializeField] Vector2 movementInput;
    [Header("Camera Controls")]
    [SerializeField] Vector2 cameraInput;
    [Header("Camera Values")]
    public float cameraInputX;
    public float cameraInputY;
    [Header("Interact Button")]
    public bool interactInput;
    [Header("Backwards Button")]
    public bool backwardsInput;

    [Header("Vert/Hor Input")]
    public float vertInput;
    public float horInput;
    private void OnEnable()
    {
        if (PlayerInput == null)
        {
            PlayerInput = new ISInputSystem();
            PlayerInput.Main.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            PlayerInput.Main.Turn.performed += i => cameraInput = i.ReadValue<Vector2>();
            PlayerInput.Main.Interact.performed += i => interactInput = true;
            PlayerInput.Main.Interact.canceled += i => interactInput = false;
            PlayerInput.Main.Backwards.performed += i => backwardsInput = true;
            PlayerInput.Main.Backwards.canceled += i => backwardsInput = false;
        }
        PlayerInput.Enable();
    }
    private void OnDisable()
    {
        PlayerInput.Disable();
    }
    public void InputHandler()
    {
        MoveInputHandler();
    }
    private void MoveInputHandler()
    {
        vertInput = movementInput.y;
        horInput = movementInput.x;
        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;
    }
}
