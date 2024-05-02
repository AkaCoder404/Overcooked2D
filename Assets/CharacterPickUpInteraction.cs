using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPickUpInteraction : MonoBehaviour
{
    private GameObject currentHoldable; // object that is currently being held
    private GameObject interactableBox;
    private bool isHolding = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            string message = isHolding ? "Dropping object" : "Picking up object";
            Debug.Log(message);
            if (!isHolding && currentHoldable != null)
            {
                PickUpObject();
            }
            else if (isHolding && interactableBox == null)
            {
                DropObject();
            }
            else if (isHolding && interactableBox != null)
            {
                PlaceObject();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HoldableObject") && !isHolding)
        {
            Debug.Log("Touching holdable object");
            currentHoldable = other.gameObject;
        }
        else if (other.CompareTag("InteractableBox"))
        {
            Debug.Log("Touching interactable box");
            interactableBox = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == currentHoldable)
        {
            currentHoldable = null;
        }
        else if (other.gameObject == interactableBox)
        {
            interactableBox = null;
        }
    }

    private void PickUpObject()
    {
        if (currentHoldable != null)
        {
            Debug.Log("Picking up object");
            // Make the holdable object a child of the character
            currentHoldable.transform.SetParent(transform);

            // Position it at the character's position
            currentHoldable.transform.position = transform.position;
            // Disable physics while holding (optional)
            Rigidbody2D rb = currentHoldable.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // Disable the non-trigger collider
            DisableColliders(currentHoldable);
            isHolding = true;
        }
    }

    private void DropObject()
    {
        if (currentHoldable != null)
        {
            Debug.Log("Dropping object");
            // Remove the object from being a child
            currentHoldable.transform.SetParent(null);

            // Re-enable physics
            Rigidbody2D rb = currentHoldable.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Re-enable the non-trigger collider
            EnableColliders(currentHoldable);
            isHolding = false;
        }
    }

    private void PlaceObject()
    {
        if (currentHoldable != null && interactableBox != null)
        {
            // Remove the object from being a child
            currentHoldable.transform.SetParent(null);

            // Position the object at the box's position
            currentHoldable.transform.position = interactableBox.transform.position;

            // Re-enable physics
            // Rigidbody2D rb = currentHoldable.GetComponent<Rigidbody2D>();
            // if (rb != null)
            // {
            //     rb.isKinematic = false;
            // }

            // Re-enable the non-trigger collider
            EnableColliders(currentHoldable);
            isHolding = false;
            Debug.Log("Object placed on box");
        }
    }

    private void DisableColliders(GameObject obj)
    {
        Collider2D[] colliders = obj.GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            if (!col.isTrigger)
            {
                col.enabled = false;
            }
        }
    }

    private void EnableColliders(GameObject obj)
    {
        Collider2D[] colliders = obj.GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            if (!col.isTrigger)
            {
                col.enabled = true;
            }
        }
    }
}


