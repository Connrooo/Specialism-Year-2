using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManagerMenuState : GameManagerBaseState
{
    public GameManagerMenuState(GameManagerStateMachine currentContext, GameManagerStateFactory gameManagerStateFactory)
    : base(currentContext, gameManagerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState() 
    {
        Ctx.inMenu= true;
        foreach(CinemachineVirtualCamera camera in Ctx.Cameras)
        {
            camera.Priority = 10;
        }
        Ctx.menuCamera.Priority = 11;
        Ctx.menuManager.GoToMenu();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Ctx.inMenu = false;
    }
    public override void CheckSwitchStates()
    {
        if (Ctx.playingGame)
        {
            SwitchState(Factory.Cutscene());
        }
    }
    public override void InitializeSubState()
    {
    }
}
