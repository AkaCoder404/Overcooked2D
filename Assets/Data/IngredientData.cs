using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "IngredientData", order = 0)]
public class IngredientData : ScriptableObject
{
    public IngredientType type;
    public float processTime = 7.4f;
    public float cookTime = 6.0f;
    // public Sprite rawSprite;

    [Header("Sprites")]
    public Sprite sprite;

    // TODO Add different sprites (or meshes) for different states (raw, processed, cooked, burnt, etc.)
}



