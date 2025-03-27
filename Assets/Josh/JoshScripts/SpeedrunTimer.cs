using UnityEngine;
using TMPro;

public class SpeedrunTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    //public TextMeshProUGUI bestTime;
    private float elapsedTime;
    private bool isRunning = false;

    public bool IsRunning()
    {
        return isRunning;
    }

    void Start()
    {
        ResetTimer();

        //float best = PlayerPrefs.GetFloat("BestTime", 0f); // Load best time
        //if (best > 0)
        //{
        //    UpdateBestTimerDisplay(best); // Pass best time instead of elapsedTime
        //}
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    public float GetTime()
    {
        return elapsedTime;
    }

    public void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 100) % 100);
        timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }

    //public void UpdateBestTimerDisplay(float elTime)
    //{
    //    int minutes = Mathf.FloorToInt(elTime / 60);
    //    int seconds = Mathf.FloorToInt(elTime % 60);
    //    int milliseconds = Mathf.FloorToInt((elTime * 100) % 100);
    //    bestTime.text = $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    //}

    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;
    public void ResetTimer()
    {
        elapsedTime = 0;
        UpdateTimerDisplay();
    }
}
