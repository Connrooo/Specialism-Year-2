using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManagerStateMachine : MonoBehaviour
{
    GameManagerBaseState currentState;
    GameManagerStateFactory states;
    public GameManagerBaseState CurrentState { get { return currentState; } set { currentState = value; } }

    [Header("Global Values")]
    public MenuManager menuManager;
    public GameObject Player;
    public GameObject currentRoomSummoned;
    public GameObject summonPointPrefab;
    public bool paused; //checks if the game is paused
    public bool playingGame; //checks if the game is being played or not (will be in menu if not being played)
    public bool canMove;
    public int day; //check the day that the player is on 1,2,3 or day 4 (the end cutscene, only used in cutscene)
    public List<int> roomsSearched; //List of rooms available to visit
    public int suspectAccused;
    public int currentRoomNumber; //Room number that player is currently investigating (0 = none selected)
    public Vector3 playerPosition;
    public bool gameFinished;
    //Quip objects
    //Power-ups


    [Header("State Values")]
    public bool inCutscene;
    public bool inHallway;
    public bool inRoom;
    public bool inDeliberation;
    public bool inMenu;

    [Header("Cinemachine")]
    public CinemachineVirtualCamera menuCamera;
    public CinemachineVirtualCamera cinematicCamera;
    public Animator animator_CinematicCamera;
    public CinemachineVirtualCamera gameplayCamera;
    public List<CinemachineVirtualCamera> Cameras;
    public bool stopAnimation;
    //[Header("Menu Values")]

    public GameObject instantiatedRoom;


    [Header("Cutscene Values")]
    public bool finishedInvestigating; //checks if the player has finished investigating or not, if not, they will be investigating.
    public bool hasRoomBeenChosen; //if true, the player will go to the room, if not, they will go to the hallway.

    [Header("Hallway Values")]
    public GameObject accessibleDoorsPrefab;
    public GameObject inaccessibleDoorsPrefab;

    [Header("Room Values")]
    public GameObject[] rooms;


    [Header("Deliberate Values")]

    public GameObject deliberationPrefab;

    public List<CluePickup> evidence0;
    public List<CluePickup> evidence1;
    public List<CluePickup> evidence2;

    public List<GameObject> caseFileImages;
    public List<GameObject> caseFileExits;

    public List<GameObject> evidenceInstances;

    public GameObject ItemsOfEvidencePrefab;
    public GameObject evidenceUIPrefab;

    [Header("Gameplay Values")]
    public List<CluePickup> pickedUpObjects;



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        states = new GameManagerStateFactory(this);
        paused = true;
    }

    private void Start()
    {
        menuManager = GameObject.FindObjectOfType<MenuManager>();
        menuCamera = GameObject.FindGameObjectWithTag("MenuCamera").GetComponent<CinemachineVirtualCamera>();
        cinematicCamera = GameObject.FindGameObjectWithTag("CinematicCamera").GetComponent<CinemachineVirtualCamera>();
        animator_CinematicCamera = GameObject.FindGameObjectWithTag("CinematicCamera").GetComponent <Animator>();
        gameplayCamera = GameObject.FindGameObjectWithTag("GameplayCamera").GetComponent<CinemachineVirtualCamera>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Cameras.Add(menuCamera);
        Cameras.Add(cinematicCamera);
        Cameras.Add(gameplayCamera);
        currentState = states.Menu();
        currentState.EnterState();
    }
    private void Update()
    {
        Debug.Log(day);
        currentState.UpdateStates();
        if (paused || !playingGame)
        {
            canMove = false;
        }
        if (!canMove ) { }
    }
}
