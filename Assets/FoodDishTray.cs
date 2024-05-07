using UnityEngine;


// Hold dishes after they are submitted (dirty)
public class FoodDishTry : Interactable
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