using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    private Rigidbody2D rb;
    private Vector2 move = new Vector2();

    // public Transform heldPoint;
    private Ingredient heldFoodItem;
    public bool IsHoldingFood { get; private set; }
    private bool isTouchingInteractable;
    private Interactable currentInteractable;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Arrow keys to move the character around
        move.x = Input.GetAxis("Horizontal") * speed;
        move.y = Input.GetAxis("Vertical") * speed;

        // Space key to pick up or place food
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttemptInteraction();
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // TODO Handle cutting and throwing food as well with a different key
            // TODO Handle washing dishes 
            // TODO Handle hold key to cut and wash dishes
        }
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
    }

    private void AttemptInteraction()
    {
        if (isTouchingInteractable && currentInteractable != null)
        {
            Debug.Log("Interacting with " + currentInteractable.name);
            currentInteractable.Interact(this);
        }
        else if (IsHoldingFood)
        {
            Debug.Log("Placing food on floor");
            PlaceFood();
        }
    }

    public void SetInteractable(Interactable interactable)
    {
        isTouchingInteractable = true;
        currentInteractable = interactable;
    }

    public void PickUpFood(Ingredient ingredientPrefab)
    {
        if (!IsHoldingFood)
        {
            Debug.Log("Picking up food");

            // Move the ingredientPrefab to the location of the player
            heldFoodItem = ingredientPrefab;
            heldFoodItem.transform.position = transform.position;
            heldFoodItem.transform.parent = transform;
            IsHoldingFood = true;
        }
    }

    public Ingredient PlaceFood()
    {
        if (IsHoldingFood && currentInteractable != null)
        {
            // Set the position of the ingredientPrefab at the interactable object location
            Debug.Log("Placing food");
            heldFoodItem.transform.position = currentInteractable.transform.position;

            // Remove the food from the player's hand
            Ingredient tempFood = heldFoodItem;

            // Unparent the food item
            heldFoodItem.transform.parent = null;

            // Set parent to the interactable object
            heldFoodItem.transform.parent = currentInteractable.transform;

            heldFoodItem = null;
            IsHoldingFood = false;
            return tempFood;
        }
        else if (IsHoldingFood)
        {
            // Drop the food on the floor
            Debug.Log("Dropping food on floor to the left of the player");
            heldFoodItem.transform.position = transform.position + Vector3.left;
            heldFoodItem.transform.parent = null;
            heldFoodItem.Drop();  // Enable physics and collider

            Ingredient tempFood = heldFoodItem;
            heldFoodItem = null;
            IsHoldingFood = false;
            return tempFood;
        }

        return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player is touching " + other.name);
        if (other.CompareTag("Interactable"))
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable != null)
            {
                SetInteractable(interactable);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            isTouchingInteractable = false;
            currentInteractable = null;
        }

    }
}