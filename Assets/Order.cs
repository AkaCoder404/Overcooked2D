using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Order : MonoBehaviour
{
    private OrderData orderData;
    public OrderData OrderData => orderData;
    public List<IngredientData> Ingredients => orderData.ingredients;

    public bool isOrderDelivered { get; private set; }
    private const float BASE_ORDER_DELIVER_TIME = 60f;
    public float InitialDeliveryRemainingTime { get; private set; } = BASE_ORDER_DELIVER_TIME;
    public float orderDeliverRemainingTime { get; private set; }
    public float orderGivenTime { get; private set; } // Arrival time of the order
    public float orderRemainingTimeWhenDelivered { get; private set; }

    public Coroutine countdownCoroutine;
    public delegate void UpdatedCountdown(float remainingTime);
    public event UpdatedCountdown OnUpdatedCountdown;

    public delegate void OrderExpired(Order order);
    public event OrderExpired OnOrderExpired;

    public delegate void OrderDelivered(Order order);
    public event OrderDelivered OnOrderDelivered;

    public void Initialize(OrderData orderData, float additionalTIme)
    {
        this.orderData = orderData;
        isOrderDelivered = false;

        InitialDeliveryRemainingTime = BASE_ORDER_DELIVER_TIME + additionalTIme;
        orderDeliverRemainingTime = InitialDeliveryRemainingTime; // et countdown time
        orderGivenTime = Time.time;
        StartCountdown();
    }

    private void StartCountdown()
    {
        countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    private void StopCountdown()
    {
        if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
    }

    // TODO Do we even need this? 
    public void ResetCountdown()
    {
        orderGivenTime = Time.time;
        orderDeliverRemainingTime = InitialDeliveryRemainingTime;
        StopCoroutine(countdownCoroutine);
        StartCountdown();
    }

    private IEnumerator CountdownCoroutine()
    {
        // TODO Alert the player that the order is about to expire

        while (orderDeliverRemainingTime > 0)
        {
            orderDeliverRemainingTime -= Time.deltaTime;
            OnUpdatedCountdown?.Invoke(orderDeliverRemainingTime);
            yield return null;
        }

        OnOrderExpired?.Invoke(this);
        // ResetCountdown();
    }

    public void SetOrderDelivered()
    {
        isOrderDelivered = true;
        orderRemainingTimeWhenDelivered = orderDeliverRemainingTime;
        StopCountdown();
        OnOrderDelivered?.Invoke(this);
    }

    // Helper method to get the order's name
    public string GetOrderName()
    {
        return orderData.orderName;
    }
}