using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDougManager : MonoBehaviour
{
    public ClueScript itemScript; 
    // Start is called before the first frame update
    void Start()
    {
        itemScript = gameObject.GetComponentInParent<ClueScript>();
    }
}
