using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderData", menuName = "OrderData", order = 2)]
public class OrderData : ScriptableObject
{
    // ScriptableObject to store order data
    public string orderName;
    public Sprite sprite; // Image of the order
    public List<IngredientData> ingredients;

    // TODO
    // public int orderTime;  // Time (in seconds) that the player has to complete the order
    // public int orderScore; // Score (coins) that the player gets for completing the order

}