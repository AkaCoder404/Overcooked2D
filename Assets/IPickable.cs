using UnityEngine;

public interface IPickable
{
    // Can be picked/dropped by a player onto the floor or onto an interactable

    GameObject gameObject { get; }
    public void PickUp();
    public void Drop();
}