using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerCutsceneState : GameManagerBaseState
{
    public GameManagerCutsceneState(GameManagerStateMachine currentContext, GameManagerStateFactory gameManagerStateFactory)
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
        Ctx.cinematicCamera.Priority = 11;
        DayChecker();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        if (Ctx.stopAnimation)
        {
            Ctx.stopAnimation = false;
            SwitchState(Factory.Hallway());
        }
    }
    public override void InitializeSubState()
    {
    }

    private void DayChecker()
    {
        switch(Ctx.day)
        {
            case 1:
                Ctx.animator_CinematicCamera.SetTrigger("day1Opening");
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

}
