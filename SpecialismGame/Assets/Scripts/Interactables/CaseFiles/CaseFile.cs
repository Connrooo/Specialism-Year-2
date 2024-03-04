using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseFile : MonoBehaviour
{
    Interactable interactable;
    [Range(0, 2)]
    [Header("Suspect that Case File Belongs Under (0 = Chef, 1 = Wife, 2 = Butler)")]
    public int suspectRelated;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.interactType = "CaseFile";
    }
}
