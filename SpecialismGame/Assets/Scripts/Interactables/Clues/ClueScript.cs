using UnityEngine;

public class ClueScript : MonoBehaviour
{
    Interactable interactable;
    public CluePickup pickup;
    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.interactType = "Clue";
    }
}
