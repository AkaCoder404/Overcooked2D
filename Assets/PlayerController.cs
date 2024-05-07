using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction dashAction;
    private InputAction pickUpAction;
    private InputAction interactAction;
    private InputAction emoteAction; // TODO

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float movementSensitivity = 0.3f;
    private Vector2 inputDirection = new();

    [SerializeField] private float dashForce = 10f;
    private bool isDashing = false;
    private bool isDashingPossible = true;
    private readonly WaitForSeconds dashDuration = new WaitForSeconds(0.17f);
    private readonly WaitForSeconds dashCooldown = new WaitForSeconds(0.07f);

    [Header("Animation")]
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Interactable currentInteractable;
    public bool isHoldingPickable { get; private set; }
    private IPickable currentPickable; // Can be an ingredient or appliance

    // Awake is called 
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();

        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        moveAction = playerInput.actions["Move"];
        dashAction = playerInput.actions["Dash"];
        pickUpAction = playerInput.actions["PickUp"];
        interactAction = playerInput.actions["Interact"];
        emoteAction = playerInput.actions["Emote"];

        moveAction.performed += HandleMove;
        dashAction.performed += HandleDash;
        pickUpAction.performed += HandlePickUp;
        interactAction.performed += HandleInteract;


    }

    // Update is called once per frame
    private void Update()
    {
        CalculateInputDirection();
    }

    // FixedUpdate is called once per physics frame
    private void FixedUpdate()
    {
        MoveThePlayer();
    }

    public void SetInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        // if (context.phase != InputActionPhase.Performed) return;

        // Calculate the move direction with sensitivity
        Vector2 tempMovement = context.ReadValue<Vector2>();

        // x direction
        if (tempMovement.x > movementSensitivity || tempMovement.x < -movementSensitivity)
        {
            tempMovement.x = Mathf.Sign(tempMovement.x) * 1f;
        }
        else
        {
            tempMovement.x = 0;
        }

        // y direction
        if (tempMovement.y > movementSensitivity || tempMovement.y < -movementSensitivity)
        {
            tempMovement.y = Mathf.Sign(tempMovement.y) * 1f;
        }
        else
        {
            tempMovement.y = 0;
        }

        inputDirection = new Vector2(tempMovement.x, tempMovement.y);
    }

    private void HandleDash(InputAction.CallbackContext context)
    {
        // if (context.phase != InputActionPhase.Performed) return;
        if (!isDashingPossible) return;
        StartCoroutine(DashAction());
    }

    private void HandlePickUp(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        // No interactable object in range
        // if (currentInteractable == null) return;

        // Empty hands, try to pick up
        if (currentPickable == null)
        {
            currentPickable = currentInteractable as IPickable;

            // Can pick up object
            if (currentPickable != null)
            {
                Debug.Log("Picking up " + currentPickable);
                currentPickable.PickUp();
                currentPickable.gameObject.transform.position = transform.position;
                currentPickable.gameObject.transform.SetParent(transform);

                return;
            }

            if (currentInteractable == null) return; // No pickable or interactable in range

            // Interacting with Interactable only (not IPickable)
            Debug.Log("Picking up from interactable");
            currentPickable = currentInteractable.PickUpFromSlot(currentPickable);
            currentPickable.gameObject.transform.position = transform.position;
            currentPickable.gameObject.transform.SetParent(transform);
            return;
        }


        // Carrying Pickable, no interactable in range
        if (currentInteractable == null || currentInteractable is IPickable)
        {
            Debug.Log("Dropping pickable on floor to the left of the player");
            currentPickable.gameObject.transform.position = transform.position + Vector3.left;
            currentPickable.Drop();
            currentPickable = null;
            return;
        }

        // Carrying Pickable, and we have an interactable in range, try to drop
        Debug.Log("Dropping pickable to interactable");
        bool dropSuccess = currentInteractable.DropToSlot(currentPickable);
        if (!dropSuccess) return;
        currentPickable = null;
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        currentInteractable?.Interact(this);
    }

    private void CalculateInputDirection()
    {
        var tempMovement = moveAction.ReadValue<Vector2>();
        // x direction
        if (tempMovement.x > movementSensitivity || tempMovement.x < -movementSensitivity)
        {
            tempMovement.x = Mathf.Sign(tempMovement.x) * 1f;
        }
        else tempMovement.x = 0;

        // y direction
        if (tempMovement.y > movementSensitivity || tempMovement.y < -movementSensitivity)
        {
            tempMovement.y = Mathf.Sign(tempMovement.y) * 1f;
        }
        else tempMovement.y = 0;

        inputDirection = new Vector2(tempMovement.x, tempMovement.y);
    }

    // Move the player based on input direction
    private void MoveThePlayer()
    {


        if (isDashing)
        {
            var currentVelocity = rb.velocity.magnitude;
            var inputNormalized = inputDirection.normalized;
            if (inputNormalized == Vector2.zero) // If the player is not moving, dash in the direction the player is facing
            {
                inputNormalized = transform.forward;
            }
            rb.velocity = inputNormalized * currentVelocity;
        }
        else rb.velocity = inputDirection.normalized * movementSpeed;

        animator.SetBool("isMoving", inputDirection != Vector2.zero); // Play walking animation
        animator.SetFloat("Horizontal", inputDirection.x);
        animator.SetFloat("Vertical", inputDirection.y);

        // Set idle animation direction, uses BlendTree just like the walking animation
        if (inputDirection != Vector2.zero)
        {
            animator.SetFloat("idleHorizontal", inputDirection.x);
            animator.SetFloat("idleVertical", inputDirection.y);
        }
    }

    private IEnumerator DashAction()
    {
        isDashingPossible = false;
        // rb.AddForce(inputDirection * dashForce, ForceMode2D.Impulse);
        // rb.AddRelativeForce(dashForce * Vector3.forward, ForceMode2D.Impulse);
        rb.AddRelativeForce(dashForce * inputDirection, ForceMode2D.Impulse);

        // TODO Play dash animation
        // yield return new WaitForFixedUpdate();
        isDashing = true;
        tr.emitting = true;
        yield return dashDuration;
        isDashing = false;
        tr.emitting = false;
        yield return dashCooldown;
        isDashingPossible = true;
    }

    // TODO Use Raycasting to detect interactable objects in front of the player
    // not just the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Player is touching " + other.name);
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
            currentInteractable = null;
        }

    }

    // private void FaceDirection(Vector2D direction)
    // {
    //     // Get the direction that the player is facing
    //     Debug.Log("Player is facing direction " + direction);
    // }
}