using UnityEngine;
using TMPro;

public class MessageBoxManager : MonoBehaviour
{

    [Header("UI Component References")]
    [Tooltip("Drag the parent GameObject of your entire message box panel here.")]
    [SerializeField] private GameObject messageBoxPanel;
    
    [Tooltip("Drag the TextMeshPro component that displays the text here.")]
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Testing Tool")]
    [TextArea(3, 5)]
    [Tooltip("Test Display Message")]
    [SerializeField] private string testMessage = "This is a custom feedback message.";

    private void Start()
    {
        CloseMessage();
    }

    private void Update()
    {
        #region Input Checks
        // Only for DEBUG & Testing purposes!
        // Press M in play mode to test message
        if (Input.GetKeyDown(KeyCode.M))
        {
            ShowMessage(testMessage);
        }

        // If the box is open and the player clicks or presses Enter/Space, close it
        if (messageBoxPanel != null && messageBoxPanel.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                CloseMessage();
            }
        }
        #endregion
    }

    /// <summary>
    /// Call this from any other script or trigger zone to display text to the player.
    /// </summary>
    public void ShowMessage(string customNewText)
    {
        if (messageBoxPanel == null || messageText == null)
        {
            Debug.LogWarning("MessageBoxManager is missing references in the Inspector!");
            return;
        }

        // Update the vertex color text string dynamically
        messageText.text = customNewText;

        // Turn the panel visual layer on
        messageBoxPanel.SetActive(true);

        // (Optional) Free up cursor if you want them to click it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Closes the message box and locks the mouse back down for standard gameplay tracking.
    /// </summary>
    public void CloseMessage()
    {
        if (messageBoxPanel != null)
        {
            messageBoxPanel.SetActive(false);
        }

        // Lock mouse cursor back down immediately for gameplay tracking
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
