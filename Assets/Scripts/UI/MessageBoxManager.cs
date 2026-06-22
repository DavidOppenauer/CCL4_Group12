using UnityEngine;
using System.Collections;
using TMPro;

// =================================================================================
// MESSAGE BOX SYSTEM - USAGE GUIDE
// =================================================================================
// 
// 1. SETUP:
//    - Drag the 'Message_Box_System' Prefab from the Prefab folder to your Scene.
//    - Ensure your scene has an 'EventSystem' component (UI -> Event System).
//
// 2. HOW TO DISPLAY A CUSTOM MESSAGE FROM OTHER SCRIPTS:
//    - Get a reference to this script component in your custom script:
//          public MessageBoxManager msgManager;
//
//    - Call the ShowMessage() function and pass your text inside quotes:
//          msgManager.ShowMessage("Your custom text goes here!");
//
// 3. PLAYER CONTROLS:
//    - Once open, the box freezes and holds its text on screen.
//    - The player can clear the message and trigger the close animation by
//      pressing: ENTER, E, SPACEbar, or by clicking ANY Mouse Button.
//    - You can test the text and animation at any time by pressing the M key.
//
// =================================================================================

public class MessageBoxManager : MonoBehaviour
{
    [Header("UI Component References")]
    [SerializeField] private GameObject messageBoxContainer; // Master Canvas
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Linked Animators (Safe if Left Empty)")]
    [Tooltip("Message_Box_Border object here.")]
    [SerializeField] private Animator borderAnimator;
    [Tooltip("Background_Panel object here.")]
    [SerializeField] private Animator panelAnimator;

    [Header("Timing Configuration")]
    [Tooltip("Time in seconds to wait for the close animations to finish before shutting off the container.")]
    [SerializeField] private float closeAnimationDuration = 0.5f;

    private bool isClosing = false;

    private void Start()
    {
        // Safety check to ensure it starts turned off on boot up
        if (messageBoxContainer != null)
        {
            messageBoxContainer.SetActive(false);
        }
    }

    private void Update()
    {
        #region Testing Tool
        if (Input.GetKeyDown(KeyCode.M)) 
        {
            ShowMessage("This is a custom Text");
            return; // Stop execution on this frame so 'M' doesn't accidentally trigger a double-input dismiss
        }
        #endregion

        #region Input Dismiss Checks
        // Only accept input if the container is active and we aren't already running the close sequence
        if (messageBoxContainer != null && messageBoxContainer.activeSelf && !isClosing)
        {
            // Triggers on: Left click (0), Right click (1), Middle click (2), Space, Enter, or E
            if (Input.GetMouseButtonDown(0) || 
                Input.GetMouseButtonDown(1) || 
                Input.GetMouseButtonDown(2) || 
                Input.GetKeyDown(KeyCode.Space) || 
                Input.GetKeyDown(KeyCode.Return) || 
                Input.GetKeyDown(KeyCode.KeypadEnter) || 
                Input.GetKeyDown(KeyCode.E))
            {
                TriggerDismissSequence();
            }
        }
        #endregion
    }

    /// <summary>
    /// Call this from any other script or trigger zone to display text and start the animation manually.
    /// </summary>
    public void ShowMessage(string customNewText)
    {
        isClosing = false; // Reset the closing flag
        
        // Null check for text mesh component
        if (messageText != null)
        {
            messageText.text = customNewText;
        }
        
        // 1. Activate the master container UI layer
        if (messageBoxContainer != null)
        {
            messageBoxContainer.SetActive(true);
        }
        
        // 2. FORCE the animations to start from code instead of playing automatically
        // Added NULL-checks here to completely wipe out the NullReferenceException errors!
        if (borderAnimator != null) borderAnimator.SetTrigger("Intro");
        if (panelAnimator != null) panelAnimator.SetTrigger("Intro");
        
        // 3. Handle the cursor tracking
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void TriggerDismissSequence()
    {
        isClosing = true;

        // Tell BOTH animators to play their exit sequences right now (Safe from null errors)
        if (borderAnimator != null) borderAnimator.SetTrigger("Dismiss");
        if (panelAnimator != null) panelAnimator.SetTrigger("Dismiss");

        // 2. Wait for the spatial scaling/fading to finish visually before killing the layout
        StartCoroutine(DisableUIComponentsDelay());
    }

    private IEnumerator DisableUIComponentsDelay()
    {
        yield return new WaitForSeconds(closeAnimationDuration);

        // Clean shutdown
        if (messageBoxContainer != null)
        {
            messageBoxContainer.SetActive(false);
        }
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isClosing = false;
    }
}