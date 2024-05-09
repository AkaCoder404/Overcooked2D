using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// MenuPanelUI atm...
// TODO Organize logic to handle multiple UI panels...
public class UIManager : Singleton<UIManager>
{
    [Header("MainMenu")] // Main Menu Panel
    [SerializeField] private GameObject mainMenuPanel;
    private CanvasGroup mainMenuCanvasGroup;

    [Header("PauseMenu")]
    [SerializeField] private GameObject pauseMenuPanel;
    private CanvasGroup pauseMenuCanvasGroup;

    [Header("Buttons")]
    [SerializeField] private GameObject firstSelectedPauseMenu; // 
    [SerializeField] private Button reStartButton; // start or restart
    [SerializeField] private Button quitButton;

    // TODO Understand delegates
    public delegate void ButtonPressed();
    public static ButtonPressed OnRestartButtonPressed;
    public static ButtonPressed OnQuitButtonPressed;

    private void Awake()
    {
        mainMenuCanvasGroup = mainMenuPanel.GetComponent<CanvasGroup>();
        pauseMenuCanvasGroup = pauseMenuPanel.GetComponent<CanvasGroup>();

        // TODO Assertions for null references

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MainMenuSetActive(true);
        }
        else
        {
            MainMenuSetActive(false);
        }

        //
        pauseMenuPanel.SetActive(false);
    }

    private void OnEnable()
    {
        AddButtonListeners();
    }

    private void OnDisable()
    {
        RemoveButtonListeners();
    }

    private void AddButtonListeners()
    {
        reStartButton.onClick.AddListener(HandleRestartButton);
        quitButton.onClick.AddListener(HandleQuitButton);
    }

    private void RemoveButtonListeners()
    {
        reStartButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }

    private static void HandleRestartButton()
    {
        Debug.Log("Restart button clicked");
        // TODO Better handle scene transitions
        SceneManager.LoadScene(1);
    }

    private static void HandleQuitButton()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }

    public static void MainMenuSetActive(bool active)
    {
        Instance.mainMenuPanel.SetActive(active);
        Instance.mainMenuCanvasGroup.alpha = active ? 1 : 0;
    }

    public static void PauseMenuSetActive(bool active)
    {
        Instance.pauseMenuPanel.SetActive(active);
        // Instance.pauseMenuCanvasGroup.alpha = active ? 1 : 0;
        // Instance.pauseMenuCanvasGroup.interactable = active;
        // Instance.pauseMenuCanvasGroup.blocksRaycasts = active;

        // if (active)
        // {
        //     EventSystem.current.SetSelectedGameObject(Instance.firstSelectedPauseMenu);
        // }
    }
    public static void PauseUnpause()
    {
        if (Instance.pauseMenuPanel.activeSelf)
        {
            PauseMenuSetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            PauseMenuSetActive(true);
            Time.timeScale = 0;
        }
    }
}