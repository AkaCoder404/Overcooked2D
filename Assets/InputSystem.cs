using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    private const string ActionMapGameplay = "PlayerControls";
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += HandleActionTriggered;
    }

    private void HandleActionTriggered(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log($"HandleActionsTriggered {context.action.name} phase: {context.action.phase}");
        }
        // Also has Started, Performed, Cancelled
    }


    public void Action(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("Action performed");
        }
        // Also has Started, Performed, Cancelled

    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        Debug.Log("Move performed");
    }
}
