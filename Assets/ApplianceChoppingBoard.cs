using UnityEngine;
using System.Collections;
using Slider = UnityEngine.UI.Slider;

public class ApplianceChoppingBoard : Interactable
{
    [SerializeField] private Slider slider;
    private Ingredient currentIngredient;
    private float finalProcessTime;
    private float currentProcessTime;
    private bool isChopping;
    private Coroutine choppingCoroutine;

    public delegate void ChoppingStatus(PlayerController playerController);
    public static event ChoppingStatus OnChoppingStart;
    public static event ChoppingStatus OnChoppingStop;

    protected override void Awake()
    {
        base.Awake();
        slider.gameObject.SetActive(false);
    }

    public override void Interact(PlayerController player)
    {
        LastPlayerControllerInteracted = player;

        // base.Interact(player);
        if (CurrentPickable == null ||
            currentIngredient == null ||
            currentIngredient.Status != IngredientStatus.Raw) return;

        if (choppingCoroutine == null)
        {
            Debug.Log("Starting chopping coroutine");
            finalProcessTime = currentIngredient.ProcessTime;
            currentProcessTime = 0f;
            slider.value = 0f;
            slider.gameObject.SetActive(true);
            StartChopCoroutine();
            return;
        }

        if (isChopping == false)
        {
            StartChopCoroutine();
        }
    }

    public override IPickable PickUpFromSlot(IPickable pickable)
    {
        if (CurrentPickable == null) return null;
        if (choppingCoroutine != null) return null;

        var tempPickable = CurrentPickable;
        currentIngredient = null;
        CurrentPickable = null;

        return tempPickable;
    }

    public override bool DropToSlot(IPickable pickable)
    {
        if (CurrentPickable != null) return false; // Not empty
        if (pickable is not Ingredient) return false; // Only ingredients can be placed on chopping board

        CurrentPickable = pickable;
        currentIngredient = pickable as Ingredient;
        CurrentPickable.gameObject.transform.position = transform.position;
        CurrentPickable.gameObject.transform.SetParent(transform);
        return true;
    }

    private void StartChopCoroutine()
    {
        OnChoppingStart?.Invoke(LastPlayerControllerInteracted);
        choppingCoroutine = StartCoroutine(Chop());
    }

    private void StopChopCoroutine()
    {
        OnChoppingStop?.Invoke(LastPlayerControllerInteracted);
        isChopping = false;
        if (choppingCoroutine != null) StopCoroutine(choppingCoroutine);
    }

    private IEnumerator Chop()
    {
        isChopping = true;
        while (currentProcessTime < finalProcessTime)
        {
            currentProcessTime += Time.deltaTime;
            slider.value = currentProcessTime / finalProcessTime;
            yield return null;
        }

        // Chopping is done
        currentIngredient.ChangeToProcessed();
        slider.gameObject.SetActive(false);
        isChopping = false;
        choppingCoroutine = null;
        OnChoppingStop?.Invoke(LastPlayerControllerInteracted);
        Debug.Log("Chopping is done");
        yield return null;
    }
}