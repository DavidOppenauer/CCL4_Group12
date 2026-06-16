using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor; // Required for SceneAsset, wrapped in an #if so the game can build
#endif

public class MainMenuController : MonoBehaviour, ISerializationCallbackReceiver
{
    #if UNITY_EDITOR
    [SerializeField] private SceneAsset gameplaySceneAsset;
    #endif

    // This string is hidden, stores the name of the scene asset
    [HideInInspector] [SerializeField] private string gameplaySceneName;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Button startButton = root.Q<Button>("BtnStart");
        Button quitButton = root.Q<Button>("BtnQuit");
        // Button optionsButton = root.Q<Button>("BtnOptions");

        if (startButton != null)
        {
            startButton.clicked += OnStartGamePressed;
        }
        else
        {
            Debug.LogError("Could not find a button named 'BtnStart' in the UXML document!");
        }

        if (quitButton != null)
        {
            quitButton.clicked += OnQuitGamePressed;
        }
        else    
        {
            Debug.LogError("Could not find a button named 'BtnQuit' in the UXML document!");
        }
    }

    private void OnDisable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        if (root != null)
        {
            Button startButton = root.Q<Button>("BtnStart");
            if (startButton != null)
            {
                startButton.clicked -= OnStartGamePressed;
            }
            Button quitButton = root.Q<Button>("BtnStart");
            if (quitButton != null)
            {
                quitButton.clicked -= OnQuitGamePressed;
            }
        }
    }

    // ==============
    // Event Listeners
    // ==============
    private void OnStartGamePressed()
    {
        // There we should put a custom loading screen ...
        Debug.Log("Starting Game... Loading " + gameplaySceneName);
        
        SceneManager.LoadScene(gameplaySceneName);
    }

    private void OnQuitGamePressed()
    {
        // Maybe add a conformation window (but not high priority)
        Debug.Log("Quitting Game...");

        Application.Quit();
    }

    // =========================================
    // AUTOMATIC NAME EXTRACTION (Serialization)
    // =========================================

    public void OnBeforeSerialize()
    {
        #if UNITY_EDITOR
            // If unity updates the inspector, extract the string name
            if (gameplaySceneAsset != null)
            {
                gameplaySceneName = gameplaySceneAsset.name;
            }
            else
            {
                gameplaySceneName = string.Empty;
            }
        #endif
    }

    public void OnAfterDeserialize()
    {
        // Interface requirement, leave blank
    }
}
