using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks; // Tasks are a way to handle async operations

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private OrderManager orderManager;
    [SerializeField] private PlayerInput playerInput;


    //
    private InputAction pauseAction;

    // 
    private static int score;
    public delegate void ScoreUpdate(int timeRemaining);
    public static ScoreUpdate OnScoreUpdate;

    // 
    private static int timeRemaining;
    public delegate void CountdownTick(int timeRemaining);
    public static CountdownTick OnCountdownTick;
    private Coroutine countdownCoroutine;
    private TimeSpan oneSecondTick = TimeSpan.FromSeconds(1);

    //
    public delegate void LevelStart();
    public static LevelStart OnLevelStart;

    //
    public delegate void TimeIsOver();
    public static TimeIsOver OnTimeIsOver;

    // Display Notifications
    public delegate void DisplayNotification(string message);
    public static DisplayNotification OnDisplayNotification;

    public static int SceneName
    {
        get => SceneManager.GetActiveScene().buildIndex;

    }

    public static int Score
    {
        get => score;
        private set
        {
            var previous = score;
            score = value;
            // OnScoreUpdate?.Invoke(score, score - previous);
        }

    }

    private void Awake()
    {
        // TODO Assertions for null references

        playerInput = GetComponent<PlayerInput>();
        pauseAction = playerInput.actions["Pause"];
        pauseAction.performed += HandlePause;
    }

    // TODO Use Task, async, and await to handle async operations
    private void Start()
    {
        GameLoop();
    }

    private void GameLoop()
    {
        // TODO
        // 1. Start level
        // 2. Countdown
        // 3. Play level
        // 4. End level
        // 5. Go to 1
        // StartMainMenu();
        StartLevel(levelData);
    }

    private void StartMainMenu()
    {
        // TODO
        // 1. Show main menu
        // 2. Wait for player input
        // 3. Hide main menu
        // 4. Start level

        UIManager.MainMenuSetActive(true); // UIManager handles button presses for now...

        // TODO
        // - make sure player can't move while in main menu
        // - enable input system controlls for main menu and remove after main menu is hidden
        // - enable player movement after main menu is hidden
    }

    private void StartLevel(LevelData levelData)
    {
        // TODO
        // 1. Show level
        // 2. Start countdown
        // 3. Play level
        // 4. End level
        // 5. Go to 1
        Score = 0;
        timeRemaining = levelData.durationTime;
        orderManager.Initialize(levelData);

        OnLevelStart?.Invoke(); // Invoke event to start level

        // CountdownTimer(timeRemaining);

        // Coroutine
        countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    private void HandlePause(InputAction.CallbackContext context)
    {
        // UIManager.PauseMenuSetActive(true);
        // Pauses player controls, time, and shows pause menu
        UIManager.PauseUnpause();
    }

    // TODO Use aysnc instead of coroutines
    private void CountdownTimer(int time)
    {
        Debug.Log("Countdown started");
        Task.Run(async () =>
        {
            while (time > 0)
            {
                await Task.Delay(oneSecondTick);
                time--;
                var timespan = TimeSpan.FromSeconds(time);
                Debug.Log("Time remaining: " + timespan.ToString(@"mm\:ss"));

                OnCountdownTick?.Invoke(time);

            }
            OnTimeIsOver?.Invoke();
        });
    }

    private IEnumerator CountdownCoroutine()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1);
            timeRemaining--;
            OnCountdownTick?.Invoke(timeRemaining);
        }

        OnTimeIsOver?.Invoke();
    }
}
