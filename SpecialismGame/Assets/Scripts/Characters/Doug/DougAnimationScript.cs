using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DougAnimationScript : MonoBehaviour
{
    public Animator dougAnimator;
    private DougAI dougAI;
    GameManagerStateMachine gameManager;
    public bool dougWalking;
    public bool dougPickup;

    // Start is called before the first frame update
    void Start()
    {
        dougAnimator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        dougAI = FindObjectOfType<DougAI>();
        if (gameManager.inHallway)
        {
            RoomAnimation(0);
        }
        else
        {
            RoomAnimation(gameManager.currentRoomNumber);
        }
    }

    // Update is called once per frame
    void Update()
    {
        IsDougWalking();
        IsDougPickingUp();
    }
    
    public void RoomAnimation(int roomNumber)
    {
        switch(roomNumber) 
        {
            case 0:
                DefaultState();
                break;
            case 1:
                DefaultState();
                break;
            case 2:
                DefaultState();
                break;
            case 3:
                HungryState();
                break;
            case 4:
                TiredState();
                break;
            case 5:
                DefaultState();
                break;
            case 6:
                HungryState();
                break;
        }
    }

    private void IsDougWalking()
    {
        if(dougWalking)
        {
            dougAnimator.SetBool("Walking", true);
        }
        else
        {
            dougAnimator.SetBool("Walking", false);
        }
    }

    private void IsDougPickingUp()
    {
        if(dougPickup)
        {
            dougAnimator.SetBool("Pickup", true);
        }
        else
        {
            dougAnimator.SetBool("Pickup", false);
        }
    }

    public void EndDougPickup()
    {
        dougPickup = false;
    }

    private void DefaultState()
    {
        dougAnimator.SetInteger("Room",0);
        dougAnimator.SetTrigger("SwitchRoom");
        dougAnimator.SetBool("Pickup", false);
        dougAnimator.SetBool("Walking", false);

    }
    private void TiredState()
    {
        dougAnimator.SetInteger("Room",1);
        dougAnimator.SetTrigger("SwitchRoom");
        dougAnimator.SetBool("Pickup", false);
        dougAnimator.SetBool("Walking", false);
    }
    private void HungryState()
    {
        dougAnimator.SetInteger("Room",2);
        dougAnimator.SetTrigger("SwitchRoom");
        dougAnimator.SetBool("Pickup", false);
        dougAnimator.SetBool("Walking", false);
    }

    public void AnimTrigger()
    {
        dougAI.FoundObject();
    }
}
