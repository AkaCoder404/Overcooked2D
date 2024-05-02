using UnityEngine;

public class FoodCrate : Interactable
{
    [SerializeField] private Ingredient ingredientPrefab;
    private Ingredient currentIngredient;

    public override void Interact(PlayerController player)
    {
        LastPlayerControllerInteracted = player;
        if (player.IsHoldingFood || currentIngredient != null)
        {
            return; // Player is already holding food or there is already food in the crate
        }

        GenerateFood(); // Generates food in the crate
        player.PickUpFood(currentIngredient); // Player picks up the food from the crate
        currentIngredient = null; // Remove the food from the crate
    }

    private void GenerateFood()
    {
        Debug.Log("Generating food called " + ingredientPrefab.name);
        currentIngredient = Instantiate(ingredientPrefab, transform.position, Quaternion.identity);
        // Set the parent of the food to the crate
        currentIngredient.transform.SetParent(transform);
    }
}