using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Don't Destroy On Loads")]
    [SerializeField] private GameObject SettingsManager;
    [SerializeField] private GameObject InputManager;
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
    [Header("First Selected Menu Objects")]
    [SerializeField] private GameObject FS_Menu;
    [SerializeField] private GameObject FS_Settings;
    [SerializeField] private GameObject FS_Pause;
    [SerializeField] private GameObject FS_Keyboard;
    [SerializeField] private GameObject FS_Gamepad;
    [SerializeField] private GameObject FS_Accessibility;
    [SerializeField] private GameObject FS_LoadGame;
    [SerializeField] private GameObject FS_Credits;
    [Header("Game State")]
    public bool gamePaused;
    public bool inGame;

    private PlayerStateMachine playerStateMachine;

    // Start is called before the first frame update
    private void Awake()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(SettingsManager);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        gamePaused = true;
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
        if (playerStateMachine.IsMenuOpenClosePressed)//pause button is pressed
        {
            if (PauseCanvas.activeSelf)//if in pause menu, resume game
            {
                P_Resume();
            }
            else if (inGame&&SettingsCanvas.activeSelf)//if in game, and settings menu, go to pause menu
            {
                SettingsCanvas.SetActive(false);
                PauseCanvas.SetActive(true);
                Debug.Log("Yippee");
            }
            else if (!inGame&& SettingsCanvas.activeSelf)//if in main menu, and settings menu, go to main menu
            {
                SettingsCanvas.SetActive(false);
                MainMenuCanvas.SetActive(true);
                Debug.Log("This is not yippee");
            }
            else if (KeyboardCanvas.activeSelf)
            {
                KeyboardCanvas.SetActive(false);
                SettingsCanvas.SetActive(true);
            }
            else if (GamepadCanvas.activeSelf)
            {
                GamepadCanvas.SetActive(false);
                SettingsCanvas.SetActive(true);
            }

            else if (AccessibilityCanvas.activeSelf)
            {
                AccessibilityCanvas.SetActive(false);
                SettingsCanvas.SetActive(true);
            }

            else if (CreditsCanvas.activeSelf)
            {
                CreditsCanvas.SetActive(false);
                MainMenuCanvas.SetActive(true);
            }
            else if (inGame && !PauseCanvas.activeSelf)//if in game, and pause menu isnt active, pause game
            {
                P_PauseGame();
            }
            else
            {
                Debug.Log("You have attempted to go back, there is no further back.");
            }
            playerStateMachine.IsMenuOpenClosePressed = false;
        }
        
        
        
        
        
        //if in keyboard controls, go to settings menu
        //if in gamepad controls, go to settings menu
    }

    public void Settings()
    {
        //if in game, hide the pause menu and pull up settings
        //if in main menu, hide the menu and pull up settings
        if (inGame)
        {
            PauseCanvas.SetActive(false);
            SettingsCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Settings);
        }
        else
        {
            MainMenuCanvas.SetActive(false);
            SettingsCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Settings);
        }
    }

    public void Back()
    {
        if (inGame && SettingsCanvas.activeSelf) //if in game and in settings, go to pause menu
        {
            SettingsCanvas.SetActive(false);
            PauseCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Pause);
        }

        else if (PauseCanvas.activeSelf) //if in game and in pause menu, go to main menu
        {
            inGame = false;
            Time.timeScale = 0f;
            //save progress to correct save, change camera to cinemachine camera.
            gamePaused = true;
            PauseCanvas.SetActive(false);
            MainMenuCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Menu);
        }
        
        else if (!inGame && SettingsCanvas.activeSelf) //if in main menu and in settings menu, go to main menu.
        {
            SettingsCanvas.SetActive(false);
            MainMenuCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Menu);
        }

        else if (GamepadCanvas.activeSelf) //if the player is in the gamepad rebinding canvas, go to the settings canvas
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

        else if (AccessibilityCanvas.activeSelf)
        {
            AccessibilityCanvas.SetActive(false);
            SettingsCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Settings);
        }

        else if (CreditsCanvas.activeSelf)
        {
            CreditsCanvas.SetActive(false);
            MainMenuCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FS_Menu);
        }

        else
        {
            Debug.Log("You have attempted to go back, there is no further back.");
        }
    }

    public void M_NewGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inGame = true;
        gamePaused = false;
        PlayerUI.SetActive(true);
        MainMenuCanvas.SetActive(false);
    }
    public void M_LoadGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inGame = true;
        gamePaused = false;
        PlayerUI.SetActive(true);
        MainMenuCanvas.SetActive(false);
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
        gamePaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        PlayerUI.SetActive(false);
        PauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(FS_Pause);
    }

    public void P_Resume()
    {
        gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerUI.SetActive(true);
        PauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }


}
