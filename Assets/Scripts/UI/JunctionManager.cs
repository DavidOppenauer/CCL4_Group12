using UnityEngine;
using System;
using System.Collections;

public class JunctionManager : MonoBehaviour
{
    [Serializable]
    public struct JunctionConfig
    {
        public bool allowLeft;
        public bool allowStraight;
        public bool allowRight;
    }

    // 1. UNCOMMENTED THESE so you can assign your UI Panels in the inspector
    [Header("Node Object References")]
    [SerializeField] private GameObject leftNode;
    [SerializeField] private GameObject straightNode;
    [SerializeField] private GameObject rightNode;

    // 2. ADDED THIS so you can check/uncheck paths directly in the Inspector!
    [Header("Test Configuration")]
    [SerializeField] private JunctionConfig testConfig;

    [Header("Animation Configuration")]
    [Tooltip("The name of the Trigger parameter connecting SlideIn to SlideOut in your Animator Controller.")]
    [SerializeField] private string disappearTriggerName = "Disappear";
    [Tooltip("Time in seconds to wait for the SlideOut animation to finish playing before turning the UI off entirely.")]
    [SerializeField] private float slideOutDuration = 0.5f;


    [Header("External UI Integration")]
    [Tooltip("Drag the CanvasGroup of your external HUD here for smooth fading.")]
    [SerializeField] private CanvasGroup healthCompassHUD;
    [SerializeField] private float fadeDuration = 0.2f;

    // Cache the Animators automatically to save performance
    private Animator leftAnimator;
    private Animator straightAnimator;
    private Animator rightAnimator;

    private void Awake()
    {
        // Safely cache components if the objects are assigned
        if (leftNode != null) leftAnimator = leftNode.GetComponent<Animator>();
        if (straightNode != null) straightAnimator = straightNode.GetComponent<Animator>();
        if (rightNode != null) rightAnimator = rightNode.GetComponent<Animator>();
    }

    // 3. QUICK UPDATE LOOP TRICK: Press J during Play Mode to instantly test your booleans!
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            OpenJunctionMenu(testConfig);
        }
    }

    /// <summary>
    /// Call this from your 3D level trigger setup to feed in which paths are active.
    /// </summary>
    public void OpenJunctionMenu(JunctionConfig config)
    {
        gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Fade OUT the external UI smoothly
        if (healthCompassHUD != null) StartCoroutine(FadeCanvasGroup(healthCompassHUD, 1f, 0f));

        if (leftNode != null) leftNode.SetActive(config.allowLeft);
        if (straightNode != null) straightNode.SetActive(config.allowStraight);
        if (rightNode != null) rightNode.SetActive(config.allowRight);
    }

    /// <summary>
    /// Automatically called whenever a direction node component gets a mouse click event.
    /// </summary>
    public void SelectDirection(string chosenDirection)
    {
        Debug.Log($"Direction Locked In: {chosenDirection}");

        // 1. Lock the mouse cursor back down immediately so standard gameplay tracking resumes
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 2. Fire the global leave action across all active nodes simultaneously
        TriggerAllActiveNodesToSlideOut();

        // 4. Safely clean up the UI gameobjects once the screen spatial wipes finish
        StartCoroutine(DisableUIComponentsAfterAnimationDelay());
    }

    private void TriggerAllActiveNodesToSlideOut()
    {
        // Check if the node object is active in the hierarchy first before screaming at its state engine
        if (leftNode != null && leftNode.activeSelf && leftAnimator != null)
            leftAnimator.SetTrigger(disappearTriggerName);

        if (straightNode != null && straightNode.activeSelf && straightAnimator != null)
            straightAnimator.SetTrigger(disappearTriggerName);

        if (rightNode != null && rightNode.activeSelf && rightAnimator != null)
            rightAnimator.SetTrigger(disappearTriggerName);
    }

    private IEnumerator DisableUIComponentsAfterAnimationDelay()
    {
        yield return new WaitForSeconds(slideOutDuration);

        if (leftNode != null) leftNode.SetActive(false);
        if (straightNode != null) straightNode.SetActive(false);
        if (rightNode != null) rightNode.SetActive(false);

        // Fade IN the external UI smoothly as the menu closes
        if (healthCompassHUD != null) yield return StartCoroutine(FadeCanvasGroup(healthCompassHUD, 0f, 1f));

        gameObject.SetActive(false);
    }

    // Simple linear interpolation helper to handle the alpha fade over time
    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end)
    {
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, time / fadeDuration);
            yield return null;
        }
        cg.alpha = end;
        
        // Disable raycasts when hidden so players can't accidentally click through it
        cg.blocksRaycasts = (end > 0.5f); 
    }
}