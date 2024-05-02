using UnityEngine;

public class FoodCountertop : Interactable
{
    public Ingredient currentFood;

    public override void Interact(PlayerController player)
    {
        Debug.Log("Interacting with countertop");
        if (player.IsHoldingFood && currentFood == null)
        {
            Debug.Log("Countertop is empty, place food on countertop");
            currentFood = player.PlaceFood();
        }
        else if (!player.IsHoldingFood && currentFood != null)
        {
            Debug.Log("Player is picking up food from countertop");
            player.PickUpFood(currentFood);
            currentFood = null;
        }
        else if (player.IsHoldingFood && currentFood != null)
        {
            Debug.Log("Countertop is not empty, cannot place food on countertop");
        }
        else
        {
            Debug.Log("Countertop is empty, player is not holding food");
        }
    }
}