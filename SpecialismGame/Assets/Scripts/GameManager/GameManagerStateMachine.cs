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
    public bool paused; //checks if the game is paused
    public bool playingGame; //checks if the game is being played or not (will be in menu if not being played)
    public bool canMove;
    public int day; //check the day that the player is on 1,2,3 or day 4 (the end cutscene, only used in cutscene)
    public int[] rooms = {1,2,3,4,5,6}; //List of rooms available to visit
    public int currentRoom; //Room player is currently investigating
    public Vector3 playerPosition;
    //public GameObject[]??? evidenceCollected;
    //Quip objects
    //Power-ups

    [Header("Cinemachine")]
    public CinemachineVirtualCamera menuCamera;
    public CinemachineVirtualCamera cinematicCamera;
    public Animator animator_CinematicCamera;
    public CinemachineVirtualCamera gameplayCamera;
    public List<CinemachineVirtualCamera> Cameras;
    public bool stopAnimation;
    //[Header("Menu Values")]


    [Header("Cutscene Values")]
    public bool finishedInvestigating; //checks if the player has finished investigating or not, if not, they will be investigating.
    public bool hasRoomBeenChosen; //if true, the player will go to the room, if not, they will go to the hallway.


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        states = new GameManagerStateFactory(this);
        paused = true;
    }

    private void Start()
    {
        menuCamera = GameObject.FindGameObjectWithTag("MenuCamera").GetComponent<CinemachineVirtualCamera>();
        cinematicCamera = GameObject.FindGameObjectWithTag("CinematicCamera").GetComponent<CinemachineVirtualCamera>();
        animator_CinematicCamera = GameObject.FindGameObjectWithTag("CinematicCamera").GetComponent <Animator>();
        gameplayCamera = GameObject.FindGameObjectWithTag("GameplayCamera").GetComponent<CinemachineVirtualCamera>();
        Cameras.Add(menuCamera);
        Cameras.Add(cinematicCamera);
        Cameras.Add(gameplayCamera);
        currentState = states.Menu();
        currentState.EnterState();
    }
    private void Update()
    {
        currentState.UpdateStates();
        if (paused || !playingGame)
        {
            canMove = false;
        }
    }
}