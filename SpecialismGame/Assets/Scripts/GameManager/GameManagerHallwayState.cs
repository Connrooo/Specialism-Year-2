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
        Ctx.inHallway= true;
        foreach (CinemachineVirtualCamera camera in Ctx.Cameras)
        {
            camera.Priority = 10;
        }
        Ctx.gameplayCamera.Priority = 11;
        Ctx.canMove = true;
        Ctx.roomDisplayValue = 0;
        if (!Ctx.loadGame)
        {
            Ctx.Player.transform.position = new Vector3(0f,1f,0f);
        }
        Ctx.loadGame = false;
        LoadHallway();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Ctx.inHallway = false;
        Object.Destroy(Ctx.currentRoomSummoned);
        if (Ctx.currentRoomNumber != 0)
        {
            Ctx.Player.transform.position = Ctx.roomPositions[Ctx.currentRoomNumber - 1];
        }
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
        Ctx.currentRoomSummoned = Object.Instantiate(Ctx.summonPointPrefab);
        GameObject summonedInaccDoors = Object.Instantiate(Ctx.inaccessibleDoorsPrefab, Ctx.currentRoomSummoned.transform);
        GameObject summonedAccDoors = Object.Instantiate(Ctx.accessibleDoorsPrefab, Ctx.currentRoomSummoned.transform);
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
                }
            }
            if (!blockedDoors.Contains(i)) //if not, it sees if it's already been checked
            {
                Object.Destroy(InaccDoorScript.doors[i]); //if it hasn't, it destroys it
            }
        }
    }
}
