using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CluePickup : MonoBehaviour
{
    [Header("Suspect that Clue Belongs Under (0 = Chef, 1 = Wife, 2 = Butler)")]
    [Range(0, 2)]
    public int suspectRelated;
    [Header("Name of the item which is being picked up")]
    public string itemName;
    [Header("Description of the item which will be on the crime report")]
    public string itemDescription;

}
