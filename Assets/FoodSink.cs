using UnityEngine;


// Sink for washing dirty dishes (and holding clean ones)

public class FoodSink : Interactable
{
    public override void Interact(PlayerController player)
    {
        LastPlayerControllerInteracted = player;
    }

    public override IPickable PickUpFromSlot(IPickable pickable)
    {
        throw new System.NotImplementedException();
    }

    public override bool DropToSlot(IPickable pickable)
    {
        throw new System.NotImplementedException();
    }
}