using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Cinemachine; 
using System.Collections; // Required for Coroutines

#if UNITY_EDITOR
using UnityEditor; 
#endif

public class MainMenuController : MonoBehaviour, ISerializationCallbackReceiver
{
    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera openingShotCamera;
    [SerializeField] private CinemachineCamera initialRailCamera;

    [Header("Priority Settings")]
    [SerializeField] private int activePriority = 20;
    [SerializeField] private int inactivePriority = 10;

    [Header("Gameplay UI")]
    [Tooltip("Gameplay UI GameObject in the hierarchy that should appear after the transition.")]
    [SerializeField] private GameObject gameplayUI;

    #if UNITY_EDITOR
    [SerializeField] private SceneAsset gameplaySceneAsset;
    #endif

    [HideInInspector] [SerializeField] private string gameplaySceneName;

    private UIDocument menuUIDocument;
    private CinemachineBrain cinemaBrain;

    private void Awake()
    {
        menuUIDocument = GetComponent<UIDocument>();
        
        cinemaBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void OnEnable()
    {
        if (menuUIDocument == null) return;
        var root = menuUIDocument.rootVisualElement;

        Button startButton = root.Q<Button>("BtnStart");
        Button quitButton = root.Q<Button>("BtnQuit");

        if (startButton != null) startButton.clicked += OnStartGamePressed;
        if (quitButton != null) quitButton.clicked += OnQuitGamePressed;

        ResetMenuState();
    }

    private void OnDisable()
    {
        if (menuUIDocument != null)
        {
            var root = menuUIDocument.rootVisualElement;
            if (root != null)
            {
                Button startButton = root.Q<Button>("BtnStart");
                if (startButton != null) startButton.clicked -= OnStartGamePressed;

                Button quitButton = root.Q<Button>("BtnQuit"); 
                if (quitButton != null) quitButton.clicked -= OnQuitGamePressed;
            }
        }
    }

    private void OnStartGamePressed()
    {
        //Debug.Log("Starting Game...");

        // 1. Give the rail camera the higher priority to start the blend
        if (openingShotCamera != null) openingShotCamera.Priority = inactivePriority;
        if (initialRailCamera != null) initialRailCamera.Priority = activePriority;

        // Hide the Main Menu UI
        if (menuUIDocument != null)
        {
            menuUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        }

        // 3. Monitor the transition state
        StartCoroutine(WaitForTransitionToFinish());
    }

    private IEnumerator WaitForTransitionToFinish()
    {
        yield return null;

        while (cinemaBrain != null && cinemaBrain.IsBlending)
        {
            yield return null;
        }

        if (cinemaBrain != null && cinemaBrain.ActiveVirtualCamera == (ICinemachineCamera)initialRailCamera)
        {
            // Debug.Log("Camera transition finished!");
            
            if (gameplayUI != null)
            {
                gameplayUI.SetActive(true);
            }
        }
    }

    private void OnQuitGamePressed()
    {
        Application.Quit();
    }

    private void ResetMenuState()
    {
        if (openingShotCamera != null) openingShotCamera.Priority = activePriority;
        if (initialRailCamera != null) initialRailCamera.Priority = inactivePriority;

        if (gameplayUI != null)
        {
            gameplayUI.SetActive(false);
        }
    }

    // =========================================
    // AUTOMATIC NAME EXTRACTION (Serialization)
    // =========================================
    public void OnBeforeSerialize()
    {
        #if UNITY_EDITOR
            if (gameplaySceneAsset != null) gameplaySceneName = gameplaySceneAsset.name;
            else gameplaySceneName = string.Empty;
        #endif
    }

    public void OnAfterDeserialize() { }
}