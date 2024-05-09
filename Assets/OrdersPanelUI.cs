using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class OrdersPanelUI : MonoBehaviour
{
    [SerializeField] private OrderCardUI orderCardPrefab;
    private List<OrderCardUI> orderCards = new List<OrderCardUI>();
    private Queue<OrderCardUI> poolOfOrderCards = new Queue<OrderCardUI>();

    private void Awake()
    {
        Debug.Log("[OrdersPanelUI] Awake");
        // TODO Assertions 
#if UNITY_EDITOR
        Assert.IsNotNull(orderCardPrefab);
#endif
    }

    private void OnEnable()
    {
        Debug.Log("[OrdersPanelUI] OnEnable");
        OrderManager.OnOrderGenerated += HandleOrderGenerated;
    }

    private void OnDisable()
    {
        OrderManager.OnOrderGenerated -= HandleOrderGenerated;
    }

    private void HandleOrderGenerated(Order order)
    {
        var rightmostOrderCard = GetRightmostOrderCardFromLastElement();
        OrderCardUI orderCardUI = GetOrderCardUIFromPool();
        orderCardUI.Initialize(order);
        orderCards.Add(orderCardUI);
        orderCardUI.SlideInSpawn(rightmostOrderCard); // Slide in from top right
    }

    // Get the X coordinate of the rightmost order card from the last element
    private float GetRightmostOrderCardFromLastElement()
    {
        if (orderCards.Count == 0) return 0f; // If there are no order cards, return 0

        // TODO Handle the case when a order is expired or delivered

        float rightmostX;
        var last = orderCards[orderCards.Count - 1]; // Get the last order card
        rightmostX = last.CurrentAnchorX + last.SizeDeltaX;
        return rightmostX;
    }

    private OrderCardUI GetOrderCardUIFromPool()
    {
        OrderCardUI orderCardUI;
        if (poolOfOrderCards.Count > 0)
        {
            orderCardUI = poolOfOrderCards.Dequeue();
        }
        else
        {
            orderCardUI = Instantiate(orderCardPrefab, transform);
        }

        return orderCardUI;
    }

    public void RegroupPnelsLeft()
    {
        Debug.Log("[OrdersPanelUI] RegroupPalensLeft");
        // float currentX = 0f;
        // foreach (var orderCard in orderCards)
        // {
        //     orderCard.SlideInSpawn(currentX);
        //     currentX += orderCard.SizeDeltaX;
        // }
    }


}