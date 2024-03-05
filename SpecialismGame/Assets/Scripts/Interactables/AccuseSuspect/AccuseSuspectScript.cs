using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuseSuspectScript : MonoBehaviour
{
    CaseFile caseFile;
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
        gameManager.finishedInvestigating= false;
    }
}
