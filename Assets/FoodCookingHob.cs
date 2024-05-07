using UnityEngine;

public class FoodCookingHob : Interactable
{
    // Awake is called when the script instance is being loaded
    protected override void Awake()
    {
        base.Awake();
        // Current Pickable is attached as child
        CurrentPickable = GetComponentInChildren<IPickable>();
        var pan = CurrentPickable as ApplianceCookingPot;
        if (pan != null) pan.isOnCookingHob = true;
    }

    public override void Interact(PlayerController player)
    {
        LastPlayerControllerInteracted = player;
        Debug.Log("Current pickable is " + CurrentPickable);
    }

    public override IPickable PickUpFromSlot(IPickable pickable)
    {
        if (CurrentPickable == null) return null; // No pickable to pick up

        var tempPickable = CurrentPickable;
        if (CurrentPickable is ApplianceCookingPot cookingPot) cookingPot.isOnCookingHob = false;
        CurrentPickable = null;
        return tempPickable;
    }

    public override bool DropToSlot(IPickable pickable)
    {
        if (CurrentPickable != null)
        {
            if (CurrentPickable is ApplianceCookingPot cookingPot1)
            {
                Debug.Log("[CookingHob] DropToSlot called on cookingPot");
                return cookingPot1.DropToSlot(pickable);
            }
            return false;
        }

        if (pickable is not ApplianceCookingPot cookingPot2) // TODO Add frying pans...
        {
            Debug.Log("Cannot drop non-cooking appliance onto hob");
            return false;
        }

        CurrentPickable = pickable;
        CurrentPickable.gameObject.transform.position = transform.position;
        CurrentPickable.gameObject.transform.SetParent(transform);

        if (CurrentPickable is ApplianceCookingPot cookingPot) cookingPot.isOnCookingHob = true;
        return true;
    }
}