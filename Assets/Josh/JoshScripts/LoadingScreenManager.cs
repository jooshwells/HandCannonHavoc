using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject levelIntro;
    private Image loadingBar;
    private float fillDuration = 3f;
    private float originalWidth;
    private RectTransform barRect;
    
    void Start()
    {
        loadingBar = GetComponent<Image>();
        barRect = loadingBar.rectTransform;
        originalWidth = barRect.sizeDelta.x;
        barRect.sizeDelta = new Vector2(0, barRect.sizeDelta.y);

        StartCoroutine(FillBar());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FillBar()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fillDuration)
        {
            float newWidth = Mathf.Lerp(0, originalWidth, elapsedTime / fillDuration);
            barRect.sizeDelta = new Vector2(newWidth, barRect.sizeDelta.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        barRect.sizeDelta = new Vector2(originalWidth, barRect.sizeDelta.y);
        levelIntro.SetActive(true);
    }
}
