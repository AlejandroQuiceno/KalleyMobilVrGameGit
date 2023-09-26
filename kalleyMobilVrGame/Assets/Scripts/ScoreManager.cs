using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] TMP_Text scoreText;
    PlayfabManager playfabManager;
    private int score;
    private void Awake()
    {
        ObjectSlicer objectSlicer = FindObjectOfType<ObjectSlicer>();
        playfabManager = FindObjectOfType<PlayfabManager>();
    }
    public void AddScore(int addedScore)
    {
        score += addedScore;
        scoreText.text = "<size=70%>X</size>" + score.ToString();
        playfabManager.SendLeaderBoard(score);
    }
    
}
