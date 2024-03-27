using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    float objectFindTimer;
    float dougWaitTimer = 180;
    GameManagerStateMachine gameManager;
    GameObject currentObject;
    GameObject objectSelected;
    InteractDougManager[] interactDougManagers;
    NavMeshAgent dougNavMesh;
    DougAnimationScript dougAnimationScript;
    Transform dougGoTo;
    Transform defaultPosition;
    Quaternion defaultRotation;
    Quaternion targetRotation;
    bool resetRotation;
    Vector3 rotSmoothDampReference;
    
    private void Start()
    {
        defaultRotation = transform.rotation;
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        interactDougManagers = FindObjectsOfType<InteractDougManager>();
        dougNavMesh = GetComponent<NavMeshAgent>();
        dougAnimationScript = FindObjectOfType<DougAnimationScript>();
        StartCoroutine(TriggerIdle());
    }

    private void Update()
    {
        DougCounter();
        IsRotationCorrect();
    }

    private void DougCounter()
    {
        if(objectFindTimer>=dougWaitTimer)
        {
            LookForObject();
        }    
        else
        {
            if (!dougAnimationScript.dougWalking)
            {
                objectFindTimer += Time.deltaTime;
            }
            else
            {
                objectFindTimer = 0;
            }
        }
    }

    private void IsRotationCorrect()
    {
        if (!dougAnimationScript.dougWalking&&resetRotation)
        {
            transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(transform.rotation.eulerAngles, targetRotation.eulerAngles, ref rotSmoothDampReference, 200 * Time.deltaTime));
            if (transform.rotation == defaultRotation)
            {
                resetRotation = false;
            }

        }
    }

    IEnumerator TriggerIdle()
    {
        yield return new WaitForSeconds(Random.Range(5,30));
        dougAnimationScript.dougAnimator.SetInteger("InteruptAnimation", Random.Range(1, 7));
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(TriggerIdle());
    }



    private void LookForObject()
    {
        if (interactDougManagers[0]!=null)
        {
            objectSelected = interactDougManagers[Random.Range(0, interactDougManagers.Length)].gameObject;
            dougGoTo = objectSelected.transform;
            dougAnimationScript.dougWalking = true;
        }
        else
        {
            objectFindTimer = 0;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform == objectSelected.transform)
        {
            dougAnimationScript.dougPickup = true;
            currentObject = objectSelected.GetComponent<InteractDougManager>().itemScript.gameObject;
            targetRotation = Quaternion.LookRotation(currentObject.transform.position);
            resetRotation = true;
        }
        else if (col.transform == defaultPosition.transform)
        {
            dougAnimationScript.dougWalking = false;
            objectFindTimer = 0;
            targetRotation = defaultRotation;
            resetRotation = true;
        }
    }

    public void FoundObject()
    {
        var clueScript = currentObject.GetComponent<ClueScript>();
        gameManager.pickedUpObjects.Add(clueScript.pickup);
        dougGoTo = defaultPosition;
        Destroy(currentObject);
        Destroy(objectSelected);
    }

    private void DougTargetPosition()
    {
        if (dougAnimationScript.dougWalking)
        {
            dougNavMesh.destination = dougGoTo.position;
        }
    }

}
