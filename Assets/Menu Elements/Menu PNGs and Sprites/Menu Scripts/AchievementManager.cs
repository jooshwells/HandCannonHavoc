using UnityEngine;
using UnityEngine.UI; // For Button component
using TMPro; // For TextMeshPro
using System.Collections;

public class AchievementSystem : MonoBehaviour
{
    // Public references (you’ll drag these into the Inspector)
    public CanvasGroup canvasGroup;       // The CanvasGroup for the panel
    public Image iconImage;               // The Image for the achievement icon
    public TextMeshProUGUI achievementHeaderText;  // Text for the achievement header
    public TextMeshProUGUI achievementExplanationText; // Text for the achievement explanation
    public Sprite[] achievementIcons;     // Array to hold all the achievement icons
    public string[] achievementHeaders;   // Array to hold all the achievement headers

    public float fadeTime;                // Time for fading in and out

    // Private variables
    private int clickCount = 0;           // Tracks how many times the button is clicked
    private bool isPopupActive = false;   // Tracks whether the popup is currently showing
    private float defaultFontSize;

    private void Start()
    {
        // Ensure panel is invisible and not interactable at the beginning
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        defaultFontSize = achievementHeaderText.fontSize;
    }

    // Public method to trigger the achievement popup
    public void TriggerAchievement()
    {
        // Prevent triggering if a popup is already playing
        if (isPopupActive) return;

        if (clickCount < achievementIcons.Length)
        {
            StartCoroutine(ShowAchievementPopup());
            clickCount++;
        }
        else
        {
            Debug.Log("All achievements unlocked!");
        }
    }

    // Coroutine for showing the popup (fade in, display message, fade out)
    private IEnumerator ShowAchievementPopup()
    {
        isPopupActive = true;

        // Set the icon and messages for the current achievement
        iconImage.sprite = achievementIcons[clickCount];
        if(clickCount == 19)
        {
            achievementHeaderText.fontSize = 21;
        }

        if(clickCount == 26)
        {
            achievementHeaderText.fontSize = 22;
        }

        else
        {
            achievementHeaderText.fontSize = defaultFontSize;
        }

        achievementHeaderText.text = "Achievement Unlocked: " + achievementHeaders[clickCount];

        if (clickCount == 0)
        {
            achievementExplanationText.text = "Click on Achievement Button 1 Time";
        }
        else
        {
            achievementExplanationText.text = "Click on Achievement Button " + (clickCount + 1) + " Times";
        }

        // Fade in the popup
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Wait while popup is fully visible
        yield return new WaitForSeconds(2f);

        // Fade out the popup
        elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        isPopupActive = false;
    }
}
