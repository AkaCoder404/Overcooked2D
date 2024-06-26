using UnityEngine;

public class Ingredient : Interactable, IPickable
{
    [SerializeField] private IngredientData data;
    private Rigidbody2D rb;
    private Collider2D col;

    public IngredientStatus Status { get; private set; }
    [SerializeField] public IngredientStatus startingStatus = IngredientStatus.Raw;
    public IngredientType Type => data.type;

    public float ProcessTime => data.processTime;
    public float CookTime => data.cookTime;

    public SpriteRenderer spriteRenderer;


    // Awake is called when the script instance is being loaded
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Setup();
    }


    private void Setup()
    {
        // Rigidbody is kinematic by default until dropped onto floor
        // Re-enable when picked up
        rb.isKinematic = true;

        // Disable collider by default until dropped onto floor
        // Re-enable when picked up
        col.enabled = false;

        Status = IngredientStatus.Raw;

        if (startingStatus == IngredientStatus.Processed)
        {
            ChangeToProcessed();
        }
    }

    public override void Interact(PlayerController player)
    {
        LastPlayerControllerInteracted = player;
    }

    public override IPickable PickUpFromSlot(IPickable pickable)
    {
        rb.isKinematic = true;
        return this;
    }

    public override bool DropToSlot(IPickable pickable)
    {
        return false;
    }

    public void PickUp()
    {
        // Disable physics
        rb.isKinematic = true;
        col.enabled = false;
    }

    public void Drop()
    {
        // Enable physics
        gameObject.transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;
    }


    public void ChangeToProcessed()
    {
        Status = IngredientStatus.Processed;
        // Change color for now
        spriteRenderer.color = Color.yellow;

        // TODO Change sprite to processed sprite
    }

    public void ChangeToCooked()
    {
        Status = IngredientStatus.Cooked;
        // Change color for now
        spriteRenderer.color = Color.green;
    }
}