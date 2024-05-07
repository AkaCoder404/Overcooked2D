using UnityEngine;

public class FoodCrate : Interactable
{
    [SerializeField] private Ingredient ingredientPrefab;

    public override void Interact(PlayerController player)
    {
        LastPlayerControllerInteracted = player;
    }

    public override IPickable PickUpFromSlot(IPickable pickable)
    {
        if (CurrentPickable == null)
        {
            return GenerateFood();
        }

        var tempPickable = CurrentPickable;
        CurrentPickable = null;
        return tempPickable;
    }

    public override bool DropToSlot(IPickable pickable)
    {
        if (CurrentPickable != null) return false;
        CurrentPickable = pickable;
        CurrentPickable.gameObject.transform.position = transform.position;
        CurrentPickable.gameObject.transform.SetParent(transform);
        return true;
    }

    private IPickable GenerateFood()
    {
        Debug.Log("Generating food called " + ingredientPrefab.name);
        return Instantiate(ingredientPrefab, transform.position, Quaternion.identity);
        // CurrentPickable.gameObject.transform.SetParent(transform);
    }
}