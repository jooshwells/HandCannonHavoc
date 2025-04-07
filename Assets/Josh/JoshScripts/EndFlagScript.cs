using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class EndFlagScript : MonoBehaviour
{
    [SerializeField] private float minTimeForNonDRank;
    [SerializeField] private int minEnemiesKilledForNonDRank;

    [SerializeField] private float minTimeForSRank;
    [SerializeField] private int minEnemiesKilledForSRank;

    [SerializeField] private GameObject levelEndHUD;
    [SerializeField] private GameObject highScoreIndicator;

    private enum ZoneNo
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4
    }

    private enum LevelNo
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }

    private SpeedrunTimer tc;
    private bool passedGate = false;
    [SerializeField] private ZoneNo zone;
    [SerializeField] private LevelNo level;
    private int numEnemiesStart = 0;
    private int numEnemiesEnd = 0;
    private bool newHighScore = false;

    private BestTimeController bt;

    // Start is called before the first frame update
    void Start()
    {
        numEnemiesStart = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log(numEnemiesStart + " enemies in level");
        tc = GameObject.Find("TimerController").GetComponent<SpeedrunTimer>();
        bt = GameObject.FindGameObjectWithTag("BestTimes").GetComponent<BestTimeController>();
    }

    public void SetPassedGate(bool p)
    {
        passedGate = p;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tc.StopTimer();
            string timeText = GameObject.FindGameObjectWithTag("ActiveTimeText").GetComponent<TextMeshProUGUI>().text;
            GameObject.FindGameObjectWithTag("RegularHUD").SetActive(false);
            levelEndHUD.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").SetActive(false);
            GameObject.FindGameObjectWithTag("EndTimeText").GetComponent<TextMeshProUGUI>().text = timeText;
            if (bt.CheckTime((int)zone, (int)level, tc.GetTime()))
            {
                newHighScore = true;
            }
            // always do normal score ui
            GameObject.FindGameObjectWithTag("TotalEnemies").GetComponent<TextMeshProUGUI>().text = numEnemiesStart.ToString();
            numEnemiesEnd = GameObject.FindGameObjectsWithTag("Enemy").Length;
            int enemiesDefeated = numEnemiesEnd <= numEnemiesStart ? Mathf.Abs(numEnemiesEnd - numEnemiesStart) : 0;
            GameObject.FindGameObjectWithTag("EnemiesKilled").GetComponent<TextMeshProUGUI>().text = enemiesDefeated.ToString();


            float enemPerc = (enemiesDefeated / (minEnemiesKilledForNonDRank != 0 ? minEnemiesKilledForNonDRank : 1)) * 100;
            float timePerc = (tc.GetTime() / (minTimeForNonDRank != 0 ? minTimeForNonDRank : 1)) * 100;
            Debug.Log("EnemPerc = " + enemPerc);
            Debug.Log("timePerc = " + timePerc);

            Animator rankingAnim = GameObject.FindGameObjectWithTag("RankingCard").GetComponent<Animator>();
            StartCoroutine(WaitToAddSuspense(enemiesDefeated, enemPerc, timePerc, rankingAnim));
            

            // trigger level end ui => pass in newHighScore, enemies defeated, time, and rating achieved
            
        }
    }

    IEnumerator WaitToAddSuspense(int enemiesDefeated, float enemPerc, float timePerc, Animator rankingAnim)
    {
        yield return new WaitForSeconds(2f);
        if (enemiesDefeated == minEnemiesKilledForSRank && tc.GetTime() <= minTimeForSRank)
        {
            // S
            rankingAnim.SetBool("S", true);
        }
        else if (enemPerc >= 100 || timePerc <= 20)
        {
            // A
            rankingAnim.SetBool("A", true);
        }
        else if (enemPerc >= 75 || timePerc <= 40)
        {
            // B
            rankingAnim.SetBool("B", true);
        }
        else if (enemPerc >= 50 || timePerc <= 60)
        {
            // C
            rankingAnim.SetBool("C", true);
        }
        else
        {
            //D
            rankingAnim.SetBool("D", true);
        }

        if (newHighScore)
        {
            highScoreIndicator.SetActive(true);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemies)
        {
            e.SetActive(false);
        }
    }
}
