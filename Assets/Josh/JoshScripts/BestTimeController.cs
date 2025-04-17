using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestTimeController : MonoBehaviour
{
    List<List<string>> zones = new List<List<string>>();

    List<string> sewerHighScores = new List<string>(); 
    List<string> bugHighScores = new List<string>();
    List<string> alienHighScores = new List<string>();
    List<string> officeHighScores = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        bugHighScores.Add("1-1");
        bugHighScores.Add("1-2");
        bugHighScores.Add("1-3");
        bugHighScores.Add("1-4");
        bugHighScores.Add("1-5");

        sewerHighScores.Add("2-1");
        sewerHighScores.Add("2-2");
        sewerHighScores.Add("2-3");
        sewerHighScores.Add("2-4");
        sewerHighScores.Add("2-5");

        alienHighScores.Add("3-1");
        alienHighScores.Add("3-2");
        alienHighScores.Add("3-3");
        alienHighScores.Add("3-4");
        alienHighScores.Add("3-5");

        officeHighScores.Add("4-1");
        officeHighScores.Add("4-2");
        officeHighScores.Add("4-3");
        officeHighScores.Add("4-4");
        officeHighScores.Add("4-5");

        zones.Add(bugHighScores);
        zones.Add(sewerHighScores);
        zones.Add(alienHighScores);
        zones.Add(officeHighScores);
    }

    // Returns true for new high score, false otherwise
    public bool CheckTime(int zone, int level, float time)
    {
        // Erkins update level player prefs here

        if (PlayerPrefs.GetFloat(zones[zone][level], float.MaxValue) > time)
        {
            PlayerPrefs.SetFloat(zones[zone][level], time);
            PlayerPrefs.Save();
            return true;
        } else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
