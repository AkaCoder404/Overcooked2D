using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Random = UnityEngine.Random;


public class OrderManager : MonoBehaviour
{
    [SerializeField] private LevelData currentLevel;
    [SerializeField] private Order orderPrefab;
    // private Order order;

    [Header("UI")]
    [SerializeField] private OrdersPanelUI ordersPanelUI;


    // Global Variables
    [Header("OrderManager Settings")]
    [SerializeField] private int MAX_CONCURRENT_ORDERS = 3;
    [SerializeField] private float ORDER_SPAWN_INTERVAL = 15f;
    [SerializeField] private float EXTRA_TIME_PER_ORDER = 20f; // Later orders will have more time to complete

    private readonly List<Order> orders = new List<Order>();
    private readonly Queue<Order> poolOfOrders = new Queue<Order>();

    private WaitForSeconds intervalBetweenDropsWait; // 
    private bool isOrderGenerationActive;
    private Coroutine orderGenerationCoroutine;


    // Events
    public delegate void OrderGenerated(Order order);
    public static event OrderGenerated OnOrderGenerated;

    public delegate void OrderDelivered(Order order);
    public static event OrderDelivered OnOrderDelivered;

    public delegate void OrderExpired(Order order);
    public static event OrderExpired OnOrderExpired;


    private void Start()
    {

    }

    public void Initialize(LevelData levelData)
    {
        currentLevel = levelData;
        Debug.Log("LevelData: " + currentLevel.name);
        Debug.Log("Level Orders: " + currentLevel.orders.Count);
        for (int i = 0; i < currentLevel.orders.Count; i++)
        {
            Debug.Log("Order: " + currentLevel.orders[i].name);
        }

        orders.Clear();
        intervalBetweenDropsWait = new WaitForSeconds(ORDER_SPAWN_INTERVAL);
        // poolOfOrders.Clear();
        isOrderGenerationActive = false;
        orderGenerationCoroutine = StartCoroutine(OrderGenerationCoroutine());
        // StartOrderGeneration();
    }

    private void StartOrderGeneration()
    {
        if (orderGenerationCoroutine != null) StopCoroutine(orderGenerationCoroutine);
        orderGenerationCoroutine = StartCoroutine(OrderGenerationCoroutine());
    }

    private void StopOrderGeneration()
    {
        if (orderGenerationCoroutine != null) StopCoroutine(orderGenerationCoroutine);
    }

    private IEnumerator OrderGenerationCoroutine()
    {
        isOrderGenerationActive = true;
        intervalBetweenDropsWait = new WaitForSeconds(ORDER_SPAWN_INTERVAL);

        while (isOrderGenerationActive)
        {
            if (orders.Count < MAX_CONCURRENT_ORDERS)
            {
                Order order = GetOrderFromPool();
                if (order == null)
                {
                    Debug.LogError("Order is null");
                    yield break;
                }
                order.Initialize(GetRandomOrderData(), orders.Count * EXTRA_TIME_PER_ORDER);
                orders.Add(order);
                // TODO SubscribeEvents
                // Print order information
                // Debug.Log("Order: " + order.name);
                // Debug.Log("Order Ingredients: " + order.OrderData.ingredients[0]);
                // Debug.Log("Order Remaining Time: " + order.orderDeliverRemainingTime);
                OnOrderGenerated?.Invoke(order);


            }

            yield return intervalBetweenDropsWait;
        }
    }

    private Order GetOrderFromPool()
    {
        if (poolOfOrders.Count == 0)
        {
            // Order order = new GameObject("Order").AddComponent<Order>();
            // order.transform.SetParent(transform);
            // poolOfOrders.Enqueue(order);
            // return order;
            // Generate a new order
            // TODO Why does it have to be a prefab?
            // Why can't we just instantiate a new Order object?
            // Answer: Prefabs are used to create new instances of objects in the scene.
            return Instantiate(orderPrefab, transform);

            // MonoBehavior can't be instantiated with new keyword
            // return new Order();
        }

        Order orderFromPool = poolOfOrders.Dequeue();
        orderFromPool.gameObject.SetActive(true);
        return orderFromPool;
    }


    private OrderData GetRandomOrderData()
    {
        var randomIndex = Random.Range(0, currentLevel.orders.Count);
        return currentLevel.orders[randomIndex];
    }
}