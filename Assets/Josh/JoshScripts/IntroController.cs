using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject regularHUD;

    [SerializeField] private GameObject lightningEffect;
    private bool coroutStarted = false;

    private bool startLevel = false; // flag for whether or not to start up the level
    private bool levelStarted = false; // bad naming, but this indicates if the level has already started to prevent the code block in update from rerunning

    [SerializeField] private GameObject loadingScreen;

    // Start is called before the first frame update
    void Start()
    {
        loadingScreen.SetActive(false);
    }
    public void SetLevelStarted(bool newStatus)
    {
        levelStarted = newStatus;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelStarted) return;

        if(!coroutStarted && lightningEffect.activeSelf)
        {
            StartCoroutine(IntroTimer());
        }

        if (startLevel)
        {
            levelStarted = true;

            // Scale Down Player
            GameObject.FindGameObjectWithTag("LevelStartPlayerHolder").GetComponent<ScaleUpPlayerLevelStart>().SetScale(false); // initiate scale down
        }
    }

    public void SpawnPlayer()
    {
        Vector3 worldPosition;
        Transform holderObject = GameObject.FindGameObjectWithTag("LevelStartPlayerHolder").transform;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            holderObject.GetComponent<RectTransform>(),
            holderObject.position,
            Camera.main,
            out worldPosition
        );
        player.transform.position = worldPosition;
        player.SetActive(true);

        holderObject.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("LevelIntroHUD").SetActive(false);
        //GameObject.FindGameObjectWithTag("RegularHUD").SetActive(true);
        regularHUD.SetActive(true);
    }

    IEnumerator IntroTimer()
    {
        yield return new WaitForSeconds(waitTime);
        startLevel = true;
    }
}
