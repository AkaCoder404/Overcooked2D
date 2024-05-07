using UnityEngine;

public class FoodTrash : Interactable
{
    public override void Interact(PlayerController player)
    {
        LastPlayerControllerInteracted = player;
    }

    public override IPickable PickUpFromSlot(IPickable pickable)
    {
        return null;
    }

    public override bool DropToSlot(IPickable pickable)
    {
        if (CurrentPickable != null) return false;

        switch (pickable)
        {
            case ApplianceCookingPot cookingPot:
                cookingPot.EmptyPot();
                break;
            case Ingredient ingredient:
                Destroy(pickable.gameObject);
                break;
            case AppliancePlate plate:
                plate.EmptyPlate();
                break;
        }
        return false;
    }

}