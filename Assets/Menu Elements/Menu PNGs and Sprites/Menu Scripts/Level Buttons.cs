using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class LevelButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Hover Sprite")]
    public Sprite hoverSprite;

    [Header("Associated Panel")]
    public GameObject levelPanel;

    private Image buttonImage;
    private Sprite originalSprite;

    private static LevelButtonHover currentlySelected = null;
    private bool isLocked = false;
    private static bool isAnyLocked;

    [Header("Hover UI Text")]
    public TMP_Text hoverText;  // Assign this in the Inspector
    public string zoneName;     // Set this per button in the Inspector


    void Start()
    {
        buttonImage = GetComponent<Image>();

        if (buttonImage != null)
            originalSprite = buttonImage.sprite;

        
        isAnyLocked = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isLocked && buttonImage != null && hoverSprite != null)
        {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
            buttonImage.sprite = hoverSprite;
        }

        if (hoverText != null && !isAnyLocked)
            hoverText.text = "You Are Currently Hovering Over: " + zoneName;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isLocked && buttonImage != null && originalSprite != null)
        {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0f);
            buttonImage.sprite = originalSprite;
        }

        if (hoverText != null && !isAnyLocked)
            hoverText.text = "";
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // If already selected, toggle it off
        if (currentlySelected == this)
        {
            isLocked = false;
            isAnyLocked = false;

            if (buttonImage != null && originalSprite != null)
            {
                buttonImage.sprite = originalSprite;
                buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0f);
            }

            if (levelPanel != null 
                && levelPanel.name != "Zone 2 Interaction Flag") // ← Do not hide this one
                levelPanel.SetActive(false);

            currentlySelected = null;
            return; // exit early
        }

        // Existing logic
        if (currentlySelected != null && currentlySelected != this)
        {
            currentlySelected.isLocked = false;

            if (currentlySelected.buttonImage != null && currentlySelected.originalSprite != null)
            {
                currentlySelected.buttonImage.sprite = currentlySelected.originalSprite;
                currentlySelected.buttonImage.color = new Color(
                    currentlySelected.buttonImage.color.r,
                    currentlySelected.buttonImage.color.g,
                    currentlySelected.buttonImage.color.b,
                    0f
                );
            }

            // ✨ Skip hiding "Zone 2 Interaction Flag"
            if (currentlySelected.levelPanel != null &&
                currentlySelected.levelPanel.name != "Zone 2 Interaction Flag")
            {
                currentlySelected.levelPanel.SetActive(false);
            }
        }

        isLocked = true;
        isAnyLocked = true;

        if (buttonImage != null && hoverSprite != null)
        {
            buttonImage.sprite = hoverSprite;
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
        }

        if (levelPanel != null)
            levelPanel.SetActive(true);

        currentlySelected = this;
        hoverText.text = "";
    }

}
