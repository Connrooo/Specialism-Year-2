using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    GameManagerStateMachine gameManager;
    SaveLoadScript saveLoadScript;

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

    [Header("Load/New Save Buttons")]
    [SerializeField] private GameObject[] loadSaveButtons;
    [SerializeField] private GameObject[] newSaveButtons;
    [SerializeField] private GameObject loadSaveText;
    [SerializeField] private GameObject newSaveText;

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
        saveLoadScript = FindObjectOfType<SaveLoadScript>();
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
        //Debug.Log(gameManager.playingGame);
        if (playerStateMachine.IsMenuOpenClosePressed)//pause button is pressed
        {
            PlayingGame();
            DefaultCheckers();
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
            Debug.Log("Hi!");
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
            if (SettingsCanvas.activeSelf) //if in game and in settings, go to pause menu
            {
                SettingsCanvas.SetActive(false);
                PauseCanvas.SetActive(true);
                EventSystem.current.SetSelectedGameObject(FS_Pause);
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
        PlayerUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
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
            saveLoadScript.Save();
        }
        PlayingGame();
        DefaultCheckers();
        
        
        //else
        //{
        //    Debug.Log("You have attempted to go back, there is no further back.");
        //}
    }

    public void M_NewGame()
    {
        MainMenuCanvas.SetActive(false);
        LoadGameCanvas.SetActive(true);
        loadSaveText.SetActive(false);
        newSaveText.SetActive(true);
        foreach (GameObject button in loadSaveButtons)
        {
            button.SetActive(false);
        }
        foreach (GameObject button in newSaveButtons)
        {
            button.SetActive(true);
        }
        EventSystem.current.SetSelectedGameObject(FS_LoadGame);
    }
    public void M_LoadGame()
    {
        MainMenuCanvas.SetActive(false);
        LoadGameCanvas.SetActive(true);
        loadSaveText.SetActive(true);
        newSaveText.SetActive(false);
        int index = 0;
        foreach (GameObject button in newSaveButtons)
        {
            button.SetActive(false);
        }
        foreach (GameObject button in loadSaveButtons)
        {
            button.SetActive(true);
            switch(index)
            {
                case 0:
                    if (PlayerPrefs.GetString("S1Room") == "")
                    {
                        button.SetActive(false);
                        newSaveButtons[index].SetActive(true);
                    }
                    break;
                case 1:
                    if (PlayerPrefs.GetString("S2Room") == "")
                    {
                        button.SetActive(false);
                        newSaveButtons[index].SetActive(true);
                    }
                    break;
                case 2:
                    if (PlayerPrefs.GetString("S3Room") == "")
                    {
                        button.SetActive(false);
                        newSaveButtons[index].SetActive(true);
                    }
                    break;
            }
            index++;
        }
        
        EventSystem.current.SetSelectedGameObject(FS_LoadGame);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameManager.playingGame = true;
        gameManager.paused = false;
        PlayerUI.SetActive(true);
        LoadGameCanvas.SetActive(false);
        subtitleManager.PlaySubtitle("Beginning");
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
