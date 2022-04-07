//Author: Craig Zeki
//Student ID: zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    //reference to the places to display the text
    [SerializeField] TMPro.TextMeshPro pointsText;
    [SerializeField] TMPro.TextMeshPro highScoreText;

    //strings
    private string pointsString = "Score\n";
    private string highStr = "High Score\n"; // replace with player initials when players are available in menu system

    //variables to track the score
    private int totalPoints = 0;
    private int highScore = 0; //stored in PlayerPrefs "HighScore"

    //static instance so that this can be accessed by all classes
    private static ScoringSystem instance;

    //instance getter which forces the instance to be set to an object reference first
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
        //subscribe to the Game System event 'OnGameRestart'
        GameSystem.Instance.onGameRestart += ResetScore;

        //load the high score from player prefs
        highScore = PlayerPrefs.GetInt("HighScore");

        //update the text
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        //update the text
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        //current score
        pointsText.text = pointsString + totalPoints;

        //high score text
        highScoreText.text = highStr + highScore;
    }

    //function which others can call to add points to the score
    public void AddPoints(int points)
    {
        //increment the score
        totalPoints += points;
        //if score is higher than the current high score, set the new high score
        if (totalPoints > highScore)
        {
            highScore = totalPoints;
            //save the high score to the player prefs
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }       
    }

    //reset the score - this is triggered by the Game System event 'OnGameRestart'
    public void ResetScore()
    {
        totalPoints = 0;
    }

    //reset the high score - currently not used - needs to be triggered by something
    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
        
    }
}
