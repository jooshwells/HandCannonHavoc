using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    void Start()
    {
        buttonImage = GetComponent<Image>();

        if (buttonImage != null)
            originalSprite = buttonImage.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isLocked && buttonImage != null && hoverSprite != null)
        {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
            buttonImage.sprite = hoverSprite;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isLocked && buttonImage != null && originalSprite != null)
        {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0f);
            buttonImage.sprite = originalSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // If already selected, toggle it off
        if (currentlySelected == this)
        {
            isLocked = false;

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

        if (buttonImage != null && hoverSprite != null)
        {
            buttonImage.sprite = hoverSprite;
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
        }

        if (levelPanel != null)
            levelPanel.SetActive(true);

        currentlySelected = this;
    }

}
