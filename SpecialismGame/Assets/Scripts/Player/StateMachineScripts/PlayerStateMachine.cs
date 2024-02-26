using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    public Transform cameraObject;
    public MenuManager menuManager;
    public GameManagerStateMachine gameManager;
    public RebindUI rebindUI;
    public SubtitleManager subtitleManager;
    //[Header("Inputs")]
    public CharacterController characterController;
    public SettingsMenu settingsMenu;
    [Header("Accessibility Settings")]
    [Range(0, 2)]
    public static int controlScheme;


    [Header("Interact Text Change")]
    public InputActionReference inputActionReference;

    [Header("Player Walk Variables")]
    public Vector3 moveDirection;
    public float movementSpeed = 5;
    

    [Header("Player Interact Variables")]
    public GameObject loadBar;
    public TMP_Text InteractPromptText;
    public Animator InteractPromptTextAnim;
    public float rayLength = 2;
    public LayerMask layerMaskInteract;
    public GameObject loadingCurrent;
    public GameObject currentObject;
    public bool loading;
    public Vector3 pointOfInterest;
    public Vector3 loadingReference;
    public float BarTransitionTime = 25f;


    [Header("Input Start Up")]
    public ISInputSystem PlayerInput;

    [Header("Movement Controls")]
    Vector2 movementInput;
    [Header("Camera Controls")]
    Vector2 cameraInput;
    [Header("Camera Values")]
    public float cameraInputX;
    public float cameraInputY;
    [Header("Interact Button")]
    public bool IsInteractPressed;
    [Header("Menu Open Close Button")]
    public bool IsMenuOpenClosePressed;
    [Header("Backwards Button")]
    public bool backwardsInput;

    [Header("Vert/Hor Input")]
    public float vertInput;
    public float horInput;

    PlayerBaseState currentState;
    PlayerStateFactory states;

    public PlayerBaseState CurrentState { get { return currentState;} set { currentState = value; } }

    private void Awake()
    {
        //playerInput = GetComponent<PlayerInput>();
        menuManager = FindObjectOfType<MenuManager>();
        settingsMenu = FindObjectOfType<SettingsMenu>();
        subtitleManager = FindObjectOfType<SubtitleManager>();
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        Debug.Log(gameManager.enabled);
        characterController = GetComponent<CharacterController>();
        menuManager.PlayerUI.SetActive(true);
        InteractPromptText = GameObject.FindWithTag("InteractPromptText").GetComponent<TMP_Text>();
        InteractPromptTextAnim = GameObject.FindWithTag("InteractPromptText").GetComponent<Animator>();
        menuManager.PlayerUI.SetActive(false);
        cameraObject = Camera.main.transform;
        states = new PlayerStateFactory(this);
        currentState = states.Idle();
        currentState.EnterState();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        currentState.UpdateStates();
    }

    private void FixedUpdate()
    {
        MoveInputHandler();
    }

    private void OnEnable()
    {
        PlayerInput = InputManager.PlayerInput;
        PlayerInput.Enable();
        PlayerInput.Main.Movement.performed += OnMove;
        PlayerInput.Main.Movement.canceled += OnMove;
        PlayerInput.Main.Turn.performed += OnTurn;
        PlayerInput.Main.Turn.canceled += OnTurn;
        PlayerInput.Main.Interact.performed += OnInteract;
        PlayerInput.Main.Interact.canceled += OnInteract;
        PlayerInput.Main.MenuOpenClose.started += OnMenuOpenClose;
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
        PlayerInput.Main.MenuOpenClose.started += OnMenuOpenClose;
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
        IsInteractPressed = ctx.ReadValueAsButton();
    }

    private void OnMenuOpenClose(InputAction.CallbackContext ctx)
    {
        IsMenuOpenClosePressed = ctx.ReadValueAsButton();
    }

    private void MoveInputHandler()
    {
        vertInput = movementInput.y;
        horInput = movementInput.x;
        cameraInputY = cameraInput.y * settingsMenu.sensitivityMultiplier;
        cameraInputX = cameraInput.x * settingsMenu.sensitivityMultiplier;
    }

    public bool CanInteract()
    {
        switch (controlScheme)
        {
            case 0:
                InteractPromptTextAnim.enabled = true;
                if (IsInteractPressed)
                {
                    Debug.Log("Pressed");
                    return true;
                }
                return false;
            case 1:
                InteractPromptTextAnim.enabled = false;
                return true;
        }
        return false;
    }
}
