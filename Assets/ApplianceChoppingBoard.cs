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

        if (player.IsHoldingFood && currentIngredient == null)
        {
            Debug.Log("Player is holding food, place it on chopping board");
            currentIngredient = player.PlaceFood();
        }
        else if (!player.IsHoldingFood && currentIngredient != null)
        {
            if (currentIngredient.Status != IngredientStatus.Raw)
            {
                // TODO Different keys to pick up vs chop
                if (currentIngredient.Status == IngredientStatus.Processed)
                {
                    Debug.Log("Player is picking up food from chopping board");
                    player.PickUpFood(currentIngredient);
                    currentIngredient = null;
                    slider.gameObject.SetActive(false);
                }

                return;
            }
            Debug.Log("Player can chop food");
            if (choppingCoroutine == null)
            {
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