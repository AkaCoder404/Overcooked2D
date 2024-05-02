using UnityEngine;

public class FoodTrash : Interactable
{
    public override void Interact(PlayerController player)
    {
        if (player.IsHoldingFood)
        {
            Debug.Log("Player is holding food, throwing it away");
            Destroy(player.PlaceFood().gameObject);
        }
        else
        {
            Debug.Log("Player is not holding food, cannot throw away");
        }
    }
}