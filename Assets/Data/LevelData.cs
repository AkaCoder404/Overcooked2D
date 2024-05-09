using System.Collections.Generic;
using UnityEngine;

// Scriptable
[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    // ScriptableObject to 
    public string levelName;
    [Tooltip("Orders that are going to be randomly spawned in the level.")]
    public List<OrderData> orders;

    public int durationTime; // Total amount of time for the level
    public int star1Score; // Score needed for 1 star
    public int star2Score; // Score needed for 2 stars
    public int star3Score; // Score needed for 3 stars

}