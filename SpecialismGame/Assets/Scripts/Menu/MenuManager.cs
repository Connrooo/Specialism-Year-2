using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    GameManagerStateMachine gameManager;


    [Header("Don't Destroy On Loads")]
    [SerializeField] private GameObject SettingsManager;
    [SerializeField] private GameObject InputManager;
    [SerializeField] private GameObject AudioManager;
    [Header("UI Canvases")]
    public GameObject PlayerUI;
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private GameObject PauseCanvas;
    [SerializeField] private GameObject KeyboardCanvas;
    [SerializeField] private GameObject GamepadCanvas;
    [SerializeField] private GameObject AccessibilityCanvas;
    [SerializeField] private GameObject LoadGameCanvas;
    [SerializeField] private GameObject CreditsCanvas;
    [SerializeField] private GameObject EndCanvas;
    [Header("First Selected Menu Objects")]
    [SerializeField] private GameObject FS_Menu;
    [SerializeField] private GameObject FS_Settings;
    [SerializeField] private GameObject FS_Pause;
    [SerializeField] private GameObject FS_Keyboard;
    [SerializeField] private GameObject FS_Gamepad;
    [SerializeField] private GameObject FS_Accessibility;
    [SerializeField] private GameObject FS_LoadGame;
    [SerializeField] private GameObject FS_Credits;

    private PlayerStateMachine playerStateMachine;
    SubtitleManager subtitleManager;

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(SettingsManager);
        DontDestroyOnLoad(InputManager);
        DontDestroyOnLoad(AudioManager);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        subtitleManager = FindObjectOfType<SubtitleManager>();
        gameManager = FindObjectOfType<GameManagerStateMachine>();
    }

    private void Start()
    {
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(FS_Menu);
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(gameManager.playingGame);
        if (playerStateMachine.IsMenuOpenClosePressed)//pause button is pressed
        {
            DefaultCheckers();
            PlayingGame();
            if (PauseCanvas.activeSelf)//if in pause menu, resume game
            {
                P_Resume();
            }
            else if (gameManager.playingGame && !PauseCanvas.activeSelf)//if in game, and pause menu isnt active, pause game
            {
                P_PauseGame();
            }
            playerStateMachine.IsMenuOpenClosePressed = false;
        }
    }

    private void DefaultCheckers()
    {
        if (GamepadCanvas.activeSelf) //if the player is in the gamepad rebinding canvas, go to the settings canvas
        {
            GamepadCanvas.SetActive(false);
            SettingsCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Settings);
        }
        else if (KeyboardCanvas.activeSelf) //if the player is in the keyboard rebinding canvas, go to the settings canvas
        {
            KeyboardCanvas.SetActive(false);
            SettingsCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Settings);
        }

        else if (AccessibilityCanvas.activeSelf) //if accessibility canvas is active, go to settings canvas
        {
            AccessibilityCanvas.SetActive(false);
            SettingsCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Settings);
        }

        else if (CreditsCanvas.activeSelf) //if credits canvas is active, go to main menu
        {
            CreditsCanvas.SetActive(false);
            MainMenuCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Menu);
        }

        else if (LoadGameCanvas.activeSelf) //if load game canvas is active, go to main menu
        {
            LoadGameCanvas.SetActive(false);
            MainMenuCanvas.SetActive(true);
        }
    }

    private void PlayingGame()
    {
        if(gameManager.playingGame)
        {
            if(!gameManager.gameFinished)
            {
                if (SettingsCanvas.activeSelf) //if in game and in settings, go to pause menu
                {
                    SettingsCanvas.SetActive(false);
                    PauseCanvas.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(FS_Pause);
                }
            }
            else
            {
                if (SettingsCanvas.activeSelf) //if in game and in settings, go to pause menu
                {
                    SettingsCanvas.SetActive(false);
                    EndCanvas.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(FS_Pause);
                }
            }
        }
        else
        {
            if (SettingsCanvas.activeSelf)
            {
                SettingsCanvas.SetActive(false);
                MainMenuCanvas.SetActive(true);
                EventSystem.current.SetSelectedGameObject(FS_Menu);
            }
        }
    }

    public void Settings()
    {
        //if in game, hide the pause menu and pull up settings
        //if in main menu, hide the menu and pull up settings
        if (PauseCanvas.activeSelf)
        {
            PauseCanvas.SetActive(false);
            SettingsCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Settings);
        }
        else if(MainMenuCanvas.activeSelf)
        {
            MainMenuCanvas.SetActive(false);
            SettingsCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Settings);
        }
    }

    public void GoToMenu()
    {
        Time.timeScale = 0f;
        gameManager.playingGame = false;
        //save progress to correct save
        gameManager.paused = true;
        PauseCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FS_Menu);
    }

    public void Back()
    {
        if (PauseCanvas.activeSelf) //if in game and in pause menu, go to main menu
        {
            gameManager.playingGame = false;
        }
        DefaultCheckers();
        PlayingGame();
        
        //else
        //{
        //    Debug.Log("You have attempted to go back, there is no further back.");
        //}
    }

    public void M_NewGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameManager.playingGame = true;
        gameManager.paused = false;
        gameManager.day = 1;
        PlayerUI.SetActive(true);
        MainMenuCanvas.SetActive(false);
        subtitleManager.PlaySubtitle("Beginning");
    }
    public void M_LoadGame()
    {
        MainMenuCanvas.SetActive(false);
        LoadGameCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FS_LoadGame);
    }


    public void M_Quit()
    {
        Application.Quit();
    }

    public void M_Credits()
    {
        MainMenuCanvas.SetActive(false);
        CreditsCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FS_Credits);
    }

    public void S_Keyboard()
    {
        SettingsCanvas.SetActive(false);
        KeyboardCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FS_Keyboard);
    }

    public void S_Gamepad() 
    {
        SettingsCanvas.SetActive(false);
        GamepadCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FS_Gamepad);
    }

    public void S_Accessibility()
    {
        SettingsCanvas.SetActive(false);
        AccessibilityCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FS_Accessibility);
    }

    public void P_PauseGame()
    {
        gameManager.paused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        PlayerUI.SetActive(false);
        PauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(FS_Pause);
    }

    public void P_Resume()
    {
        gameManager.paused = false;
        PlayerUI.SetActive(true);
        PauseCanvas.SetActive(false);
        bool inCaseFile = false;
        foreach(GameObject caseImage in gameManager.caseFileImages)
        {
            if (caseImage.activeSelf) { inCaseFile = true; }
        }
        if (!inCaseFile) 
        {
            gameManager.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        Time.timeScale = 1f;
    }
}
