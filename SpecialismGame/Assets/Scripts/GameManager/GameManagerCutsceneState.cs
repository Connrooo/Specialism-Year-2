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
        Ctx.inCutscene= true;
        foreach (CinemachineVirtualCamera camera in Ctx.Cameras)
        {
            camera.Priority = 10;
        }
        Ctx.cinematicCamera.Priority = 11;
        Ctx.canMove = false;
        DayChecker();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Ctx.inCutscene= false;
    }
    public override void CheckSwitchStates()
    {
        if (Ctx.stopAnimation)
        {
            if(Ctx.day>=4)
            {
                Ctx.gameFinished= true;
            }
            else
            {
                Ctx.stopAnimation = false;
                if (Ctx.finishedInvestigating)
                {
                    SwitchState(Factory.Deliberate());
                }
                else
                {
                    SwitchState(Factory.Hallway());
                }
            }
        }
    }
    public override void InitializeSubState()
    {
    }

    private void DayChecker()
    {
        Ctx.animator_CinematicCamera.SetInteger("day", Ctx.day);
        Ctx.animator_CinematicCamera.SetInteger("suspect", Ctx.suspectAccused);
        if (Ctx.finishedInvestigating)
        {
            Ctx.animator_CinematicCamera.SetBool("opening", true);
        }
        else
        {
            Ctx.animator_CinematicCamera.SetBool("opening", false);
        }
        Ctx.animator_CinematicCamera.SetTrigger("activate");
    }

}
