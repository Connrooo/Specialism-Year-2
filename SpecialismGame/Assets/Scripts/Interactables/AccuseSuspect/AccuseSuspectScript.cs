using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuseSuspectScript : MonoBehaviour
{
    public CaseFile caseFile;
    AccuseButton accuseButtonScript;
    GameManagerStateMachine gameManager;

    private void Start()
    {
        caseFile = GetComponent<CaseFile>();
        accuseButtonScript = FindObjectOfType<AccuseButton>();
        gameManager= FindObjectOfType<GameManagerStateMachine>();
    }

    public void AccuseSuspect()
    {
        gameManager.suspectAccused = caseFile.suspectRelated;
        gameManager.day = 3;
        gameManager.finishedInvestigating= false;
    }
}
