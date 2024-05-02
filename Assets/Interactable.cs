using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public delegate void InteractionHandler(Interactable interactable);
    public event InteractionHandler OnInteractionStart;
    public event InteractionHandler OnInteractionEnd;

    protected PlayerController LastPlayerControllerInteracted;
    protected IPickable CurrentPickable;

    // Awake is called when the script instance is being loaded
    protected virtual void Awake() { }

    // Method to be called when the player interacts with the object
    public abstract void Interact(PlayerController player);

    // (Optional) Handle interaction stop if needed
    public virtual void StopInteract(PlayerController player)
    {
        OnInteractionEnd?.Invoke(this);
    }

    protected void StartInteraction(PlayerController player)
    {
        OnInteractionStart?.Invoke(this);
    }

    protected void EndInteraction(PlayerController player)
    {
        OnInteractionEnd?.Invoke(this);
    }
}