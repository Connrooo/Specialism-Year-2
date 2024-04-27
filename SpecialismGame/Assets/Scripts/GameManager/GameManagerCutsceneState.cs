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
        Ctx.stopInteract = true;
        Ctx.animator_CinematicCamera.SetBool("playing", true);
        Ctx.interactPromptText.SetBool("TurnOff", true);
        AudioManager.Instance.DialogueSource.Stop();
        DayChecker();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Ctx.stopAnimation = false;
        Ctx.inCutscene= false;
        Ctx.stopInteract = false;
        Ctx.animator_CinematicCamera.SetBool("playing", false);
        Ctx.animator_CinematicCamera.transform.GetComponent<CinematicCamera>().detectiveAnimator.SetBool("playing", false);
        Ctx.interactPromptText.SetBool("TurnOff", false);
        AudioManager.Instance.StopMusic();
    }
    public override void CheckSwitchStates()
    {
        if (!Ctx.playingGame)
        {
            Ctx.canMove = false;
            SwitchState(Factory.Menu());
            Ctx.fadingScript.overrideFade = true;
        }
        else if (Ctx.stopAnimation)
        {
            if(Ctx.day>=4)
            {
                Ctx.playingGame = false;
                Ctx.saveLoadScript.Save();
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
            Ctx.animator_CinematicCamera.SetBool("opening", false);
            Ctx.roomDisplayValue = 2;
        }
        else
        {
            Ctx.animator_CinematicCamera.SetBool("opening", true);
        }
        if (Ctx.day == 4)
        {
            Ctx.roomDisplayValue = 3;
        }
        Ctx.animator_CinematicCamera.SetTrigger("activate");
        Ctx.audioManager.RunCutscene(true);
        foreach (GameObject np in Ctx.newspapers)
        {
            np.SetActive(false);
        }
        Ctx.newspapers[Ctx.suspectAccused].SetActive(true);
    }

}
