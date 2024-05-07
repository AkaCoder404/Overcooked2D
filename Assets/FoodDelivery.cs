using UnityEngine;

public class FoodDelivery : Interactable
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
        var platePickable = pickable as AppliancePlate;
        if (platePickable == null) return false; // Can't drop anything other than plate

        // TODO Animation and Order System
        platePickable.EmptyPlate();
        platePickable.SetDirty();
        platePickable.gameObject.SetActive(false);
        throw new System.NotImplementedException();
    }

}