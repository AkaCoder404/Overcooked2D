using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Lean.Transition;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Assertions;

public class OrderCardUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform cardPanel;
    [SerializeField] private RectTransform cardPanelBase;
    [SerializeField] private Image orderImage;
    [SerializeField] private Slider countdownSlider;
    [SerializeField] private List<Image> ingredientImages = new List<Image>();
    [SerializeField] private Image[] images; // 

    public Order Order { get; private set; }

    private float initialRemainingTime;

    private const float UI_WIDTH = 150; // width of the entire card

    //
    public float CurrentAnchorX { get; private set; } // 
    public float SizeDeltaX => cardPanel.sizeDelta.x; // 



    private void Awake()
    {
        cardPanel = GetComponent<RectTransform>();
        cardPanelBase = GetComponent<RectTransform>();
        // countdownSlider = GetComponentInChildren<Slider>();
        // TODO Assertions
#if UNITY_EDITOR
        Assert.IsNotNull(countdownSlider);
#endif
    }

    public void Initialize(Order order)
    {
        Order = order;

        // Set the panel to the right of the screen
        Debug.Log("Screen width: " + Screen.width);
        cardPanel.anchoredPosition = new Vector2(Screen.width + 300f, 0f);

        // 
        var sizeDelta = cardPanel.sizeDelta;
        cardPanel.sizeDelta = new Vector2(UI_WIDTH, sizeDelta.y);

        // 
        initialRemainingTime = order.InitialDeliveryRemainingTime;


        // TODO Set the order image


        // TODO Set the ingredient images




        // 

        // SubscribeEvents to handle AlertTime, ExpiredTime, HandleDelivered, HandleUpdatedCountdown
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        Order.OnUpdatedCountdown += HandleUpdatedCountdown;
        // Order.OnOrderExpired += HandleOrderExpired;
        // Order.OnOrderDelivered += HandleOrderDelivered;
    }

    private void UnsubscribeEvents()
    {
        Order.OnUpdatedCountdown -= HandleUpdatedCountdown;
        // Order.OnOrderExpired -= HandleOrderExpired;
        // Order.OnOrderDelivered -= HandleOrderDelivered;
    }

    private void HandleUpdatedCountdown(float remainingTime)
    {
        // Debug.Log("Remaining time: " + remainingTime);
        countdownSlider.value = remainingTime / initialRemainingTime;
    }

    // Handle slide in from top right
    public void SlideInSpawn(float desiredX)
    {
        CurrentAnchorX = desiredX;
        float initialSlideDuration = 1.5f;

        Vector2 small = new Vector2(0.8f, 1f);

        // TODO Understand lean transition
        // Slide card from right to left
        cardPanel
            .anchoredPositionTransition_x(desiredX, initialSlideDuration, LeanEase.Decelerate);
        // .JoinTransition();
        // .PlaySoundTransition(popAudio);

        // Bouncing effect
        cardPanelBase.
            localRotationTransition(Quaternion.identity, initialSlideDuration, LeanEase.Decelerate)
            .JoinTransition()
            .localScaleTransition_xy(small, 0.125f, LeanEase.Elastic)
            .JoinTransition()
            .localScaleTransition_xy(Vector2.one, 0.125f, LeanEase.Smooth);
    }
}