using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class AppliancePlate : Interactable, IPickable
{
    private Rigidbody2D rb;
    private Collider2D col;

    // TODO UI for plate
    [SerializeField] public TMPro.TextMeshPro textMeshPro;


    private const int MAX_INGREDIENTS = 4;
    private List<Ingredient> Ingredients { get; } = new List<Ingredient>(MAX_INGREDIENTS);
    public bool IsClean = true;
    public bool IsEmpty() => Ingredients.Count == 0;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // TODO UI for plate
        textMeshPro = GetComponentInChildren<TMPro.TextMeshPro>();
        textMeshPro.text = $"{Ingredients.Count}/{MAX_INGREDIENTS}";

        rb.isKinematic = true;
        col.enabled = false;
    }

    public override void Interact(PlayerController player)
    {
        LastPlayerControllerInteracted = player;
    }

    public override IPickable PickUpFromSlot(IPickable pickable)
    {
        Debug.Log("[Plate] PickUpFromSlot: " + pickable);
        // We can pick up Ingredients from plates or with plates
        if (pickable == null) return null; // Can't pick up stuff 

        switch (pickable)
        {
            case Ingredient ingredient:
                Debug.Log("Plate: Try to pick up ingredient into plate");
                break;
            case ApplianceCookingPot cookingPot:
                Debug.Log("Plate: Try to pick up from Cookingpot into plate");
                break;
            case AppliancePlate plate:
                Debug.Log("Plate: Trying to pick up stuff from another platei nto plate");
                break;

        }
        return null;
    }

    public override bool DropToSlot(IPickable pickable)
    {
        if (pickable == null) return false; // Can't drop nothing
        if (Ingredients.Count == MAX_INGREDIENTS) return false; // Plate is full

        switch (pickable)
        {
            case Ingredient ingredient: // TODO refactor
                Debug.Log("Plate: Placing ingredient on plate");
                ingredient.gameObject.transform.position = transform.position;
                ingredient.gameObject.transform.SetParent(transform);
                ingredient.gameObject.SetActive(false);
                Ingredients.Add(ingredient);
                textMeshPro.text = $"{Ingredients.Count}/{MAX_INGREDIENTS}";
                return true;
            case AppliancePlate plate: // TODO
                Debug.Log("Plate: Swap plate contents");
                break;
            case ApplianceCookingPot cookingPot: // TODO
                Debug.Log("Plate: Placing pot contents onto plate");
                // If soup, can only add one type of soup
                AddIngredients(cookingPot.Ingredients);
                break;
            default:
                Debug.Log("Plate: Can't place " + pickable + " onto plate");
                return false;
        }
        return false;
    }

    public void PickUp()
    {
        rb.isKinematic = true;
        col.enabled = false;
    }

    public void Drop()
    {
        gameObject.transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;
    }

    // TODO Add checks to control whether or not ingredients can be added
    public bool AddIngredients(List<Ingredient> ingredients)
    {
        if (!IsEmpty()) return false; // Plate is not empty

        Ingredients.AddRange(ingredients);

        foreach (var ingredient in Ingredients)
        {
            ingredient.transform.SetParent(transform);
            ingredient.transform.position = transform.position;
        }

        // TODO 
        textMeshPro.text = $"{Ingredients.Count}/{MAX_INGREDIENTS}";
        return true;
    }

    public void SetClean()
    {
        IsClean = true;
        textMeshPro.text = $"{Ingredients.Count}/{MAX_INGREDIENTS}";
    }

    public void SetDirty()
    {
        IsClean = false;
        textMeshPro.text = $"{Ingredients.Count}/{MAX_INGREDIENTS}";
    }

    public void EmptyPlate()
    {
        Ingredients.Clear();
        textMeshPro.text = $"{Ingredients.Count}/{MAX_INGREDIENTS}";
    }


    // TODO More check functions for specific orders
}