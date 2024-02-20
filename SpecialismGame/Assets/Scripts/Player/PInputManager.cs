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

    private void FixedUpdate()
    {
        InputHandler();
    }

        private void OnEnable()
    {
        PlayerInput = new ISInputSystem();
        PlayerInput.Enable();
        PlayerInput.Main.Movement.performed += OnMove;
        PlayerInput.Main.Movement.canceled += OnMove;
        PlayerInput.Main.Turn.performed += OnTurn;
        PlayerInput.Main.Turn.canceled += OnTurn;
        PlayerInput.Main.Interact.performed += OnInteract;
        PlayerInput.Main.Interact.canceled += OnInteract;
    }

    private void OnDisable()
    {
        PlayerInput.Disable();
        PlayerInput.Main.Movement.performed -= OnMove;
        PlayerInput.Main.Movement.canceled -= OnMove;
        PlayerInput.Main.Turn.performed -= OnTurn;
        PlayerInput.Main.Turn.canceled -= OnTurn;
        PlayerInput.Main.Interact.performed -= OnInteract;
        PlayerInput.Main.Interact.canceled -= OnInteract;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    private void OnTurn(InputAction.CallbackContext ctx)
    {
        cameraInput = ctx.ReadValue<Vector2>();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        interactInput = ctx.ReadValueAsButton();
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
