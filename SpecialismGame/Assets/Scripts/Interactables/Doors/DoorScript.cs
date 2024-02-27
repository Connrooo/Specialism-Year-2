using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [Header("The number of the room which is being opened)")]
    [Header("0 = None, 1 = Living Room, 2 = Bathroom, 3 = Kitchen")]
    [Header("4 = Bedroom, 5 = Study Room, 6 = Dining Room")]
    public int roomNumber;
}