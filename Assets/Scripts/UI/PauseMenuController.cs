using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenuController : MonoBehaviour, ISerializationCallbackReceiver
{
    [Header("Scene Configuration")]
    #if UNITY_EDITOR
    [SerializeField] private SceneAsset mainMenuSceneAsset;
    #endif
    [HideInInspector] [SerializeField] private string mainMenuSceneName;

    private UIDocument pauseUIDocument;
    private VisualElement pauseScreenRoot;
    private Button resumeButton;
    private Button quitButton;
    private bool isPaused = false;
    
    // Controls whether the player can currently bring up the pause screen
    private bool isAllowedToPause = false;

    private void Awake()
    {
        pauseUIDocument = GetComponent<UIDocument>();
        SetupUIElements();
    }

    private void OnEnable()
    {
        if (pauseScreenRoot == null) SetupUIElements();
        RegisterButtonEvents();
        ForceUnpause();

        // NEW: Check if a Main Menu controller exists anywhere in this scene
        MainMenuController mainMenu = Object.FindFirstObjectByType<MainMenuController>();
        
        if (mainMenu != null)
        {
            // A main menu exists! Stay locked and let the main menu handle the unlock timing.
            isAllowedToPause = false;
        }
        else
        {
            // No main menu found! We are in a standalone gameplay scene, unlock immediately.
            isAllowedToPause = true;
        }
    }

    private void OnDisable()
    {
        UnregisterButtonEvents();
    }

    private void Update()
    {
        // If we aren't allowed to pause yet (still in main menu), ignore input entirely
        if (!isAllowedToPause) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    // Public function that the MainMenuController calls to unlock pausing
    public void EnablePauseFeature(bool enable)
    {
        isAllowedToPause = enable;
    }

    private void SetupUIElements()
    {
        if (pauseUIDocument == null || pauseUIDocument.rootVisualElement == null) return;
        pauseScreenRoot = pauseUIDocument.rootVisualElement.Q<VisualElement>("PauseScreen");
        if (pauseScreenRoot != null)
        {
            resumeButton = pauseScreenRoot.Q<Button>("BtnResume");
            quitButton = pauseScreenRoot.Q<Button>("BtnQuitToMenu");
        }
    }

    private void RegisterButtonEvents()
    {
        if (resumeButton != null) resumeButton.clicked += ResumeGame;
        if (quitButton != null) quitButton.clicked += QuitToMainMenu;
    }

    private void UnregisterButtonEvents()
    {
        if (resumeButton != null) resumeButton.clicked -= ResumeGame;
        if (quitButton != null) quitButton.clicked -= QuitToMainMenu;
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseScreenRoot != null)
        {
            pauseScreenRoot.style.display = DisplayStyle.Flex;
            pauseScreenRoot.Blur(); 
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseScreenRoot != null)
        {
            pauseScreenRoot.style.display = DisplayStyle.None;
        }
    }

    private void QuitToMainMenu()
    {
        Time.timeScale = 1f; 
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    private void ForceUnpause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseScreenRoot != null)
        {
            pauseScreenRoot.style.display = DisplayStyle.None;
        }
    }

    public void OnBeforeSerialize()
    {
        #if UNITY_EDITOR
            if (mainMenuSceneAsset != null) mainMenuSceneName = mainMenuSceneAsset.name;
            else mainMenuSceneName = string.Empty;
        #endif
    }
    public void OnAfterDeserialize() { }
}