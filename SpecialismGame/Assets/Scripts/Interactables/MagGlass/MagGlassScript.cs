using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagGlassScript : MonoBehaviour
{
    private GameManagerStateMachine gameManager;
    public List<GameObject> pointers;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
    }
    private void FinishedTimer()
    {
        foreach (GameObject p in pointers) 
        {
            Destroy(p);
        }
        gameManager.magGlassActive = false;
        Destroy(gameObject);
    }
}
