using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadingLookAt : MonoBehaviour
{
    InteractScript interactScript;
    [SerializeField] GameObject Player;
    Camera cameraMain;
    GameObject currentObject;

    private void Awake()
    {
        cameraMain = Camera.main;
        Player = GameObject.FindWithTag("Player");
        interactScript = Player.GetComponent<InteractScript>();
    }
    void Update()
    {
        transform.LookAt(cameraMain.transform.position);
        if (interactScript.controlScheme == 1)
        {
            transform.position = interactScript.pointOfInterest;
        }
    }

    void Kill()
    {
        interactScript.loading = false;
        Destroy(interactScript.currentObject);
        Destroy(gameObject);
    }
}
