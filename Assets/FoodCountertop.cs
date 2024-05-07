// Countertop to place food and appliances

using UnityEngine;

public class FoodCountertop : Interactable
{
    protected override void Awake()
    {
        CurrentPickable = GetComponentInChildren<IPickable>();
    }
    public override void Interact(PlayerController player)
    {
        LastPlayerControllerInteracted = player;
    }

    public override IPickable PickUpFromSlot(IPickable pickable)
    {
        if (CurrentPickable == null)
        {
            Debug.Log("Countertop is empty, cannot pick up pickable");
            return null;
        }

        var tempPickable = CurrentPickable;
        CurrentPickable = null;
        return tempPickable;
    }

    public override bool DropToSlot(IPickable pickable)
    {
        if (CurrentPickable != null)
        {
            // TODO Drop onto type of CurrentPickable (e.g CookingPot, ChoppingBoard, Plate, etc.)
            if (CurrentPickable is AppliancePlate plate)
            {
                // TODO Limited things can be placed on plate (e.g. Soup and Burger parts cannot both be on plate at the same time)
                return plate.DropToSlot(pickable);
            }

            return false;
        }

        // Empty countertop, drop pickable directly
        CurrentPickable = pickable;
        CurrentPickable.gameObject.transform.position = transform.position;
        CurrentPickable.gameObject.transform.SetParent(transform);
        return true;
    }
}