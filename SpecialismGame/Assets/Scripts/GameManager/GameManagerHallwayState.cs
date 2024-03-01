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
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        if (!Ctx.playingGame)
        {
            Ctx.canMove = false;
            SwitchState(Factory.Menu());
        }
    }
    public override void InitializeSubState()
    {
    }

    private void LoadHallway()
    {
        GameObject summonedInaccDoors = Transform.Instantiate(Ctx.inaccessibleDoorsPrefab, Ctx.summonPoint);
        GameObject summonedAccDoors = Transform.Instantiate(Ctx.accessibleDoorsPrefab, Ctx.summonPoint);
        var InaccDoorScript = summonedInaccDoors.GetComponent<DoorPrefabScript>();
        var AccDoorScript = summonedAccDoors.GetComponent<DoorPrefabScript>();
        foreach (int room in Ctx.rooms)
        {
            for (int i = 0; i < InaccDoorScript.doors.Count; i++)
            {
                if (room == i + 1)
                {
                    Object.Destroy(InaccDoorScript.doors[i]);
                }
            }
        }
        for (int i = 0; i < AccDoorScript.doors.Count; i++)
        {
            //have a bool and turn it true if the value is in array, if it isnt at the end, kill the door.
        }
    }
}
