using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseFileExit : MonoBehaviour
{
    GameManagerStateMachine gameManager;
    PlayerStateMachine playerManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        playerManager = FindObjectOfType<PlayerStateMachine>();
    }

    public void ExitCaseFile()
    {
        var caseNumber = playerManager.currentObject.GetComponent<CaseFile>().suspectRelated;
        gameManager.caseFileImages[caseNumber].SetActive(false);
        gameManager.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
