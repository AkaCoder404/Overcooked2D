using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        // TextMeshPro is a child of the GameObject
        countDownText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // OnEnable is used to subscribe to events, called when the object is enabled
    private void OnEnable()
    {
        GameManager.OnCountdownTick += HandleCountDownTick;
        GameManager.OnLevelStart += HandleLevelStart;
        GameManager.OnTimeIsOver += HandleTimeIsOver;

    }

    private void OnDisable()
    {
        GameManager.OnCountdownTick -= HandleCountDownTick;
        GameManager.OnLevelStart -= HandleLevelStart;
        GameManager.OnTimeIsOver -= HandleTimeIsOver;
    }

    public void SetCountDownText(int timeRemaining)
    {
        var timespan = TimeSpan.FromSeconds(timeRemaining);
        countDownText.text = timespan.ToString(@"mm\:ss");
    }

    private void HandleCountDownTick(int timeRemaining)
    {
        SetCountDownText(timeRemaining);
    }

    private void HandleLevelStart()
    {
        canvasGroup.alpha = 1f;
    }

    private void HandleTimeIsOver()
    {
        canvasGroup.alpha = 0f;
    }
}