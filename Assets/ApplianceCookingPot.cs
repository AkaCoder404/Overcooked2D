using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Slider = UnityEngine.UI.Slider;
using TMPro;

public class ApplianceCookingPot : Interactable, IPickable
{
    [Header("UI")]
    [SerializeField] private Slider slider;
    [Tooltip("green popup icon for done cooking")]
    [SerializeField] private Sprite donePopupIcon;
    [SerializeField] private Sprite burnedPopupIcon;

    // TODO TextMesh Counter for UI (is a child of the pot) TextMeshPro - Text 
    [SerializeField] private TMPro.TextMeshPro textMeshPro;

    // Timers
    private float finalCookTime;
    private float currentCookTime;

    private float finalBurnTime;
    private float currentBurnTime;

    private Coroutine cookingCoroutine;
    private Coroutine burningCoroutine;

    // Flags
    private const int MAX_INGREDIENTS = 3;
    public bool isOnCookingHob;
    private bool isCooking;
    private bool isBurning;

    // 
    private Rigidbody2D rb;
    private Collider2D col;

    public bool IsCookingFinished { get; private set; }
    public bool IsBurned { get; private set; }
    public List<Ingredient> Ingredients = new List<Ingredient>(MAX_INGREDIENTS);
    public bool isPotEmpty => Ingredients.Count == 0;

    // Temp
    [SerializeField] private Ingredient tempIngredientPrefab;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Temp ----------
        for (int i = 0; i < MAX_INGREDIENTS; i++)
        {
            Ingredients.Add(Instantiate(tempIngredientPrefab, transform.position, Quaternion.identity));
            // turn off physics
            Ingredients[i].PickUp();
            Ingredients[i].transform.SetParent(transform);
            Ingredients[i].gameObject.SetActive(false);
            Ingredients[i].ChangeToProcessed();
            // Debug.Log(Ingredients.Count);
        }
        IsBurned = false;
        IsCookingFinished = true;
        // Temp --------------


        // TODO
        textMeshPro = GetComponentInChildren<TMPro.TextMeshPro>();
        textMeshPro.text = $"{Ingredients.Count}/{MAX_INGREDIENTS}";
        // Rigidbody is kinematic by default until dropped onto floor
        // Re-enable when picked up
        rb.isKinematic = true;
        col.enabled = false;
    }

    public override void Interact(PlayerController player)
    {
        LastPlayerControllerInteracted = player;
    }

    public override IPickable PickUpFromSlot(IPickable pickable)
    {
        Debug.Log("Trying to pickup from Pot");
        // Ensure pot is finished cooking
        if (!IsCookingFinished && Ingredients.Count != MAX_INGREDIENTS) return null;

        // Cannot pick up if soup has different ingredients (e.g. onion soup, tomato soup)
        // Gameplay currently does not support mixing ingredients
        if (Ingredients[0].Type != Ingredients[1].Type) return null;
        if (Ingredients[1].Type != Ingredients[2].Type) return null;
        if (Ingredients[0].Type != Ingredients[2].Type) return null;

        // Can only pick up with plate pickable
        if (!(pickable is AppliancePlate plate)) return null;

        // Plate has to be clean and empty
        if (!plate.IsClean || !plate.IsEmpty()) return null;

        Debug.Log("Picking up soup with plate...");
        plate.AddIngredients(Ingredients);
        EmptyPot();
        return null;
    }

    public override bool DropToSlot(IPickable pickable)
    {
        switch (pickable)
        {
            case Ingredient ingredient: // Drop ingredient onto pot
                if (ingredient.Status != IngredientStatus.Processed) return false;
                if (ingredient.Type == IngredientType.Onion || ingredient.Type == IngredientType.Tomato || ingredient.Type == IngredientType.Mushroom)
                {
                    if (Ingredients.Count >= MAX_INGREDIENTS) return false; // Pot is full

                    Debug.Log("Drop ingredient onto pot");

                    Ingredients.Add(ingredient);
                    finalCookTime += ingredient.CookTime;

                    // TODO Meshes, textures, animations
                    ingredient.gameObject.transform.position = transform.position;
                    ingredient.gameObject.transform.SetParent(transform);
                    // Hide ingredient
                    ingredient.gameObject.SetActive(false);

                    textMeshPro.text = $"{Ingredients.Count}/{MAX_INGREDIENTS}";

                    // TODO Burn timer
                    currentBurnTime = 0f; // Reset burn timer

                    // (re)start cooking
                    if (isOnCookingHob && !isCooking)
                    {
                        cookingCoroutine = StartCoroutine(Cook());
                    }

                    return true;
                }
                return false;
            case AppliancePlate plate:
                // If CookingPot is empty, try to dump plate (soup) ingredients

                Debug.Log("[CookingPot] Trying to get pot into plate");
                // If Cooking is finished
                if (IsCookingFinished && !IsBurned)
                {
                    if (!plate.IsClean) return false;
                    Debug.Log("[CookingPot] Adding to plate");
                    plate.AddIngredients(this.Ingredients);
                    EmptyPot();
                    return false;
                }
                break;
            default:
                Debug.Log("Pot: Can't place " + pickable + " onto pot");
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
        // Enable Physics
        gameObject.transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;

    }

    private void AddIngredients()
    {

    }

    public void EmptyPot()
    {
        if (cookingCoroutine != null) StopCoroutine(cookingCoroutine);
        if (burningCoroutine != null) StopCoroutine(burningCoroutine);

        slider.gameObject.SetActive(false);
        Ingredients.Clear();

        currentCookTime = 0f;
        currentBurnTime = 0f;
        finalCookTime = 0f;
        IsBurned = false;
        IsCookingFinished = false;
        isCooking = false;
        isBurning = false;

        // TODO
        textMeshPro.text = $"{Ingredients.Count}/{MAX_INGREDIENTS}";
    }

    private void DropToHob() { }
    private void PickUpFromHob() { }

    private IEnumerator Cook()
    {
        isCooking = true;
        slider.gameObject.SetActive(true);

        while (currentCookTime < finalCookTime)
        {
            currentCookTime += Time.deltaTime;
            slider.value = currentCookTime / finalCookTime;
            yield return null;
        }
        // Cooking is done
        isCooking = false;
        if (Ingredients.Count == MAX_INGREDIENTS)
        {
            IsCookingFinished = true;
            currentCookTime = 0f;
            // TODO Start Burn Coroutine
            Debug.Log("Successfully cooked ingredients");
            yield break;
        }

        // TODO Start Burn coroutine
    }

    private IEnumerator Burn()
    {
        yield return null;
    }


}