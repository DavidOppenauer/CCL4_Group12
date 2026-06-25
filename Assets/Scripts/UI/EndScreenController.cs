using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EndScreenController : MonoBehaviour, ISerializationCallbackReceiver
{
    [Header("Scene Configuration")]
    #if UNITY_EDITOR
    [SerializeField] private SceneAsset mainMenuSceneAsset;
    #endif
    [HideInInspector] [SerializeField] private string mainMenuSceneName;

    private UIDocument endScreenUIDocument;
    private Button returnMenuButton;

    private void Awake()
    {
        endScreenUIDocument = GetComponent<UIDocument>();
        SetupUIElements();
    }

    private void OnEnable()
    {
        if (returnMenuButton == null) SetupUIElements();
        
        if (returnMenuButton != null)
        {
            returnMenuButton.clicked += OnReturnToMenuPressed;
        }

        // Clean UI layout check: Lock mouse display so player can select navigation options
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        if (returnMenuButton != null)
        {
            returnMenuButton.clicked -= OnReturnToMenuPressed;
        }
    }

    private void SetupUIElements()
    {
        if (endScreenUIDocument == null || endScreenUIDocument.rootVisualElement == null) return;

        // Query the button by name
        returnMenuButton = endScreenUIDocument.rootVisualElement.Q<Button>("BtnReturnToMenu");
    }

    private void OnReturnToMenuPressed()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.Log($"Returning to Main Menu: {mainMenuSceneName}");
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("Main Menu scene asset reference is missing on the EndScreenController script!");
        }
    }

    // =========================================================
    // AUTOMATIC NAME EXTRACTION (Serialization)
    // =========================================================
    public void OnBeforeSerialize()
    {
        #if UNITY_EDITOR
            if (mainMenuSceneAsset != null) mainMenuSceneName = mainMenuSceneAsset.name;
            else mainMenuSceneName = string.Empty;
        #endif
    }

    public void OnAfterDeserialize() { }
}