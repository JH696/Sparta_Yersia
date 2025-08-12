using UnityEngine;

[SerializeField]
public interface IInteractable
{
    void Interact(GameObject interactor);

    string InteractText();
}