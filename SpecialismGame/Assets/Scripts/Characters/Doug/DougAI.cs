using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.AI;

public class DougAI : MonoBehaviour
{
    float objectFindTimer;
    public float dougWaitTimer = 180;
    GameManagerStateMachine gameManager;
    GameObject currentObject;
    GameObject objectSelected;
    [SerializeField] List<InteractDougManager> interactDougManagers;
    NavMeshAgent dougNavMesh;
    DougAnimationScript dougAnimationScript;
    Vector3 dougGoTo;
    public GameObject squatColliderPrefab;
    GameObject squatCollider;
    Quaternion defaultRotation;
    Quaternion targetRotation;
    bool resetRotation;
    Vector3 rotSmoothDampReference;
    
    private void Start()
    {
        defaultRotation = transform.rotation;
        squatCollider = GameObject.FindGameObjectWithTag("SquatObject");
        var tempPos = transform.position;
        tempPos.y++;
        squatCollider.transform.position = tempPos;
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        interactDougManagers = FindObjectsOfType<InteractDougManager>().ToList();
        dougNavMesh = GetComponent<NavMeshAgent>();
        dougAnimationScript = FindObjectOfType<DougAnimationScript>();
        StartCoroutine(TriggerIdle());
    }

    private void Update()
    {
        for (int i = 0; i < interactDougManagers.Count; i++)
        {
            if (interactDougManagers[i].transform == null)
            {
                interactDougManagers.Remove(interactDougManagers[i]);
            }
        }
        DougCounter();
        IsRotationCorrect();
        DougTargetPosition();
    }

    private void DougCounter()
    {
        if(objectFindTimer>=dougWaitTimer)
        {
            Debug.Log("StartedLooking");
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
        if (resetRotation)
        {
            transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(transform.rotation.eulerAngles, targetRotation.eulerAngles, ref rotSmoothDampReference, 50 * Time.deltaTime));
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
        dougAnimationScript.dougAnimator.SetInteger("InteruptAnimation", 0);
        StartCoroutine(TriggerIdle());
    }



    private void LookForObject()
    {
        if (interactDougManagers[0]!=null)
        {
            int i = Random.Range(0, interactDougManagers.Count);
            objectSelected = interactDougManagers[i].gameObject;
            interactDougManagers.Remove(interactDougManagers[i]);
            dougGoTo = objectSelected.transform.position;
            dougAnimationScript.dougWalking = true;
            objectFindTimer = 0;
        }
        else
        {
            objectFindTimer = 0;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (objectSelected!=null)
        {
            dougAnimationScript.dougPickup = true;
            currentObject = objectSelected.GetComponent<InteractDougManager>().itemScript.gameObject;
            targetRotation = Quaternion.LookRotation(currentObject.transform.position);
            resetRotation = true;
        }
        else if (col.transform.CompareTag("SquatObject"))
        {
            Debug.Log("Hehehe");
            dougAnimationScript.dougWalking = false;
            objectFindTimer = 0;
            targetRotation = defaultRotation;
            resetRotation = true;
        }
        else if (objectSelected==null)
        {
            Debug.Log("Oh");
            dougGoTo = squatCollider.transform.position;
            dougAnimationScript.dougPickup = false;
        }
    }

    public void FoundObject()
    {
        var clueScript = currentObject.GetComponent<ClueScript>();
        gameManager.pickedUpObjects.Add(clueScript.pickup);
        dougGoTo = squatCollider.transform.position;
        Destroy(currentObject);
        Destroy(objectSelected);
        dougAnimationScript.dougPickup = false;
    }

    private void DougTargetPosition()
    {
        if (dougAnimationScript.dougWalking)
        {
            Debug.Log("What");
            dougNavMesh.destination = dougGoTo;
        }
    }

}
