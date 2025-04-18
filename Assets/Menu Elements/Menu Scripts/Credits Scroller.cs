using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroller : MonoBehaviour
{
    public float scrollSpeed = 40f;
    private RectTransform rectTransform;

    [Header("Return Prompt")]
    public GameObject returnPrompt; // Assign your "Press E" UI Text here
    public float showPromptDelay = 5f;
    private bool canReturn = false;

    [Header("Return Scene")]
    public string menuSceneName = "MainMenu";

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (returnPrompt != null)
            returnPrompt.SetActive(false);

        StartCoroutine(EnableReturnPromptAfterDelay());
    }

    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        if (canReturn && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(menuSceneName);
        }
    }

    IEnumerator EnableReturnPromptAfterDelay()
    {
        yield return new WaitForSeconds(showPromptDelay);

        if (returnPrompt != null)
            returnPrompt.SetActive(true);

        canReturn = true;
    }
}
