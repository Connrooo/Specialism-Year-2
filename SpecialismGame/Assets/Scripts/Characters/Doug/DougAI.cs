using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    public bool dougWalking;
    float objectFindTimer;
    float dougWaitTimer = 180;
    GameManagerStateMachine gameManager;
    GameObject currentObject;
    GameObject objectSelected;
    InteractDougManager[] interactDougManagers;
    NavMeshAgent dougNavMesh;
    DougAnimationScript dougAnimationScript;
    
    private void Start()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        interactDougManagers = FindObjectsOfType<InteractDougManager>();
        dougNavMesh = GetComponent<NavMeshAgent>();
        dougAnimationScript = GetComponent<DougAnimationScript>();
    }

    private void Update()
    {
        DougCounter();
    }

    private void DougCounter()
    {
        if(objectFindTimer>=dougWaitTimer)
        {
            LookForObject();
        }    
        else
        {
            if (!dougWalking)
            {
                objectFindTimer += Time.deltaTime;
            }
            else
            {
                objectFindTimer = 0;
            }
        }
    }

    private void LookForObject()
    {
        objectSelected = interactDougManagers[Random.Range(0, interactDougManagers.Length)].gameObject;
        dougNavMesh.destination = objectSelected.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == objectSelected.transform)
        {
            dougAnimationScript.dougPickup = true;
        }
    }

    private void FoundObject()
    {
        var clueScript = currentObject.GetComponent<ClueScript>();
        gameManager.pickedUpObjects.Add(clueScript.pickup);
        Destroy(currentObject);
        Destroy(objectSelected);
    }

}
