using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform destination;

    public void Interact()
    {
        GameManager.Instance.Player.transform.position = destination.position;
    }
}