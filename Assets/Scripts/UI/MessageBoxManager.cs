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
    private bool canDismiss = false;

    private void Start()
    {
        if (messageBoxContainer != null)
        {
            messageBoxContainer.SetActive(false);
        }
    }

    private void Update()
    {
        if (messageBoxContainer != null && messageBoxContainer.activeSelf && !isClosing && canDismiss)
        {
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
    }

    public void ShowMessage(string customNewText)
    {
        AkUnitySoundEngine.PostEvent("Play_UI_Beam_Message_In", gameObject);
        isClosing = false;
        canDismiss = false;

        if (messageText != null)
        {
            messageText.text = customNewText;
        }

        if (messageBoxContainer != null)
        {
            messageBoxContainer.SetActive(true);
        }

        if (borderAnimator != null) borderAnimator.SetTrigger("Intro");
        if (panelAnimator != null) panelAnimator.SetTrigger("Intro");

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(AllowDismissNextFrame());
    }

    private IEnumerator AllowDismissNextFrame()
    {
        yield return null;
        canDismiss = true;
    }

    public void TriggerDismissSequence()
    {
        AkUnitySoundEngine.PostEvent("Play_UI_Beam_Message_Out", gameObject);
        isClosing = true;
        canDismiss = false;

        if (borderAnimator != null) borderAnimator.SetTrigger("Dismiss");
        if (panelAnimator != null) panelAnimator.SetTrigger("Dismiss");

        StartCoroutine(DisableUIComponentsDelay());
    }

    private IEnumerator DisableUIComponentsDelay()
    {
        yield return new WaitForSeconds(closeAnimationDuration);

        if (messageBoxContainer != null)
        {
            messageBoxContainer.SetActive(false);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isClosing = false;
    }

    public bool GetIsMessageOpen()
    {
        return messageBoxContainer != null && messageBoxContainer.activeSelf;
    }

    public void ShowPrompt(string promptText)
    {
        isClosing = false;
        canDismiss = false;

        if (messageText != null)
        {
            messageText.text = promptText;
        }

        if (messageBoxContainer != null)
        {
            messageBoxContainer.SetActive(true);
        }

        if (borderAnimator != null) borderAnimator.SetTrigger("Intro");
        if (panelAnimator != null) panelAnimator.SetTrigger("Intro");

        // Important: do NOT unlock cursor for simple prompts
    }

    public void HidePrompt()
    {
        if (messageBoxContainer != null && messageBoxContainer.activeSelf && !isClosing)
        {
            TriggerDismissSequence();
        }
    }
}