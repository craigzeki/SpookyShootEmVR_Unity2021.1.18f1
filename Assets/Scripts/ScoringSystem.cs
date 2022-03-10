using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro pointsText;
    [SerializeField] TMPro.TextMeshPro highScoreText;

    private string pointsString = "Score\n";
    private string highStr = "High Score\n"; // replace with player initials when players are available in menu system

    private int totalPoints = 0;
    private int highScore = 0; //stored in PlayerPrefs "HighScore"

    private static ScoringSystem instance;

    public static ScoringSystem Instance { 
        get 
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ScoringSystem>();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameSystem.Instance.onGameRestart += ResetScore;
        highScore = PlayerPrefs.GetInt("HighScore");
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
       
        UpdateScoreText();
        //CheckAchievements();
    }

    private void UpdateScoreText()
    {
        //current score
        pointsText.text = pointsString + totalPoints;


        //high score text
        highScoreText.text = highStr + highScore;

        
    }

    public void AddPoints(int points)
    {
        totalPoints += points;
        if (totalPoints > highScore)
        {
            highScore = totalPoints;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }       
    }

    public void ResetScore()
    {
        totalPoints = 0;
    }

    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();

    }

    //private void CheckAchievements()
    //{
    //    switch(totalPoints)
    //    {
    //        case 1:
    //            AchievementManager.Instance.EarnAchievement("My 1st Surface");
    //            break;

    //        case 5:
    //            AchievementManager.Instance.EarnAchievement("Clear 5 Surfaces");
    //            break;

    //        case 10:
    //            AchievementManager.Instance.EarnAchievement("Clear 10 Surfaces");
    //            break;

    //        case 15:
    //            AchievementManager.Instance.EarnAchievement("Clear 15 Surfaces");
    //            break;

    //        case 30:
    //            AchievementManager.Instance.EarnAchievement("Clear 30 Surfaces");
    //            break;
    //    }

    //    //Check the time achievements - note only works as elapsedTime has been rounded to an int and will be a whole number for 1 second
    //    //This is much nicer than nested if's though!
    //    switch(elapsedTime)
    //    {
    //        case 30:
            
    //            AchievementManager.Instance.EarnAchievement("Last 30 Seconds");
    //            break;
    //        case 60:
    //            AchievementManager.Instance.EarnAchievement("Last 60 Seconds");
    //            break;
    //        case 90:
    //            AchievementManager.Instance.EarnAchievement("Last 90 Seconds");
    //            break;

    //    }
        

    //}
}
