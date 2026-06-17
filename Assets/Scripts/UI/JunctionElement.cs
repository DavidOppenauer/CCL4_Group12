using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Image))]
public class JunctionElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Target Configuration")]
    [Tooltip("Drag the GameObject whose Image color you want to change here.")]
    [SerializeField] private GameObject targetGameObject;

    [Header("Visual Properties")]
    [SerializeField] private Color defaultColor = Color.black;
    [SerializeField] private Color hoverColor = Color.cyan;
    [SerializeField] private TextMeshProUGUI textObject;

    private Color defaultFontColor = Color.white;
    private Color hoverFontColor = Color.black;
    private Image targetImage;
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        
        // Find and cache the Image component on the target GameObject
        if (targetGameObject != null)
        {
            targetImage = targetGameObject.GetComponent<Image>(); 
        }
        else
        {
            // Fallback: If you leave the slot empty, it will just use itself
            targetImage = GetComponent<Image>();
        }
        
        // Initialize to default visual state
        if (targetImage != null)
        {
            targetImage.color = defaultColor;
        }

        if (textObject != null)
        {
            textObject.color = defaultFontColor;
        }
    }

    // Triggered automatically when the mouse hovers OVER the element
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.color = hoverColor;    
        }
        if (textObject != null)
        {
            textObject.color = hoverFontColor;
        }
    }

    // Triggered automatically when the mouse LEAVES the element
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.color = defaultColor;
        }
        if(textObject != null)
        {
            textObject.color = defaultFontColor;
        }
        transform.localScale = originalScale; 
    }

    // Triggered automatically when the user CLICKS the element
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Junction Node Selected: {gameObject.name}");
    }

    private void OnDisable()
    {
        if (targetImage != null) targetImage.color = defaultColor;
        transform.localScale = originalScale;
    }
}