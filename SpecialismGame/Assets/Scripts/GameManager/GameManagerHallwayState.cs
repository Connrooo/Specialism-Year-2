using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerHallwayState : GameManagerBaseState
{
    public GameManagerHallwayState(GameManagerStateMachine currentContext, GameManagerStateFactory gameManagerStateFactory)
    : base(currentContext, gameManagerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState() 
    {
        foreach (CinemachineVirtualCamera camera in Ctx.Cameras)
        {
            camera.Priority = 10;
        }
        Ctx.gameplayCamera.Priority = 11;
        Ctx.canMove = true;
        LoadHallway();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Debug.Log("Exited Hallway State");
    }
    public override void CheckSwitchStates()
    {
        if (!Ctx.playingGame)
        {
            Ctx.canMove = false;
            SwitchState(Factory.Menu());
        }
        else if(Ctx.hasRoomBeenChosen)
        {
            SwitchState(Factory.Room());
        }
    }
    public override void InitializeSubState()
    {
    }

    private void LoadHallway()
    {
        GameObject summonedInaccDoors = Object.Instantiate(Ctx.inaccessibleDoorsPrefab, Ctx.summonPoint);
        GameObject summonedAccDoors = Object.Instantiate(Ctx.accessibleDoorsPrefab, Ctx.summonPoint);
        var InaccDoorScript = summonedInaccDoors.GetComponent<DoorPrefabScript>();
        var AccDoorScript = summonedAccDoors.GetComponent<DoorPrefabScript>();
        List<int> blockedDoors = new List<int>();
        for (int i = 0; i < InaccDoorScript.doors.Count; i++) //for every possible door place
        {
            if (Ctx.roomsSearched.Count == 0) //if no rooms have been searched
            {
                Object.Destroy(InaccDoorScript.doors[i]); //destroy door (will repeat 6 times)
            }
            else
            {
                foreach (int room in Ctx.roomsSearched) //for every room
                {
                    if (i + 1 == room) //check to see if it has been searched
                    {
                        Object.Destroy(AccDoorScript.doors[i]); //if it has, it gets blocked
                        blockedDoors.Add(i);
                    }
                    else
                    {
                        if (blockedDoors.Contains(i + 1)) //if not, it sees if it's already been checked
                        {
                            Object.Destroy(InaccDoorScript.doors[i]); //if it hasn't, it destroys it
                        }
                    }
                }
            }
        }
    }
}
