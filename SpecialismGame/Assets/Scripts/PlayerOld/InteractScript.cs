using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class InteractScript : MonoBehaviour
{
    Camera Cam;
    PlayerMotion playerMotion;
    PInputManager pInputManager;
    ControlSchemeState controlSchemeState;
    [SerializeField] GameObject loadBar;
    [SerializeField] GameObject InteractUI;
    
    [Header("Object Detection")]
    public static bool canDetect;
    [SerializeField] private float rayLength = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;
    
    [Header("Loading Bar")]
    GameObject loadingCurrent;
    public GameObject currentObject;
    public bool loading;
    public Vector3 pointOfInterest;

    void Awake()
    {
        pInputManager = FindObjectOfType<PInputManager>();
        controlSchemeState = FindObjectOfType<ControlSchemeState>();
        Cam = Camera.main;
    }

    private void Update()
    {
        ContinuousRaycast();
    }

    public void InteractHandler()
    {
        switch (controlSchemeState.controlScheme)
        {
            case 0:
                canDetect = pInputManager.interactInput;
                break;
            case 1:
                canDetect = true;
                break;
            case 2:
                canDetect = true;
                break;
        }
        InputChecker();

    }
    private void InputChecker()
    {
        InteractRaycast();
        ContinuousRaycast();
    }

    private void ContinuousRaycast()
    {
        RaycastHit hit;
        Vector3 front = Cam.transform.TransformDirection(Vector3.forward);
        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;
        Debug.DrawRay(Cam.transform.position, front * rayLength, Color.green);

        if (Physics.Raycast(Cam.transform.position, front, out hit, rayLength, mask))
        {
            pointOfInterest = hit.point;
            if (hit.collider.CompareTag("Interact"))
            {
                Debug.Log("Detected: " + hit.collider.gameObject.name);
                InteractUI.SetActive(true);
            }
            else 
            {
                InteractUI.SetActive(false);
            }
        }
        else 
        {
            InteractUI.SetActive(false);
        }
    
    }

    private void InteractRaycast()
    {
        RaycastHit hit;
        Vector3 front = Cam.transform.TransformDirection(Vector3.forward);
        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;
        Debug.DrawRay(Cam.transform.position, front, Color.green);
        if (Physics.Raycast(Cam.transform.position, front, out hit, rayLength, mask))
        {
            pointOfInterest = hit.point;
            if (canDetect)
            {
                switch (hit.collider.tag)
                {
                    case "Interact":
                        if (!loading)
                        {

                            currentObject = hit.transform.gameObject;
                            loading = true;
                            LoadingBar();
                        }
                        break;
                }
            }
            if (hit.collider.tag != "Interact" || !canDetect)
            {
                NotInteracting();
            }
        }
        else
        {
            NotInteracting();
        }
    }
   
    void NotInteracting()
    {
        loading = false;
        Destroy(loadingCurrent);
    }

    void LoadingBar()
    {
        loadingCurrent = Instantiate(loadBar, new Vector3(pointOfInterest.x, pointOfInterest.y, pointOfInterest.z), Quaternion.identity);
    }
}