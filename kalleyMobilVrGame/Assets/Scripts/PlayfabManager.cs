using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayfabManager : MonoBehaviour
{
    private void Start()
    {
        Login();
    }
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            
            CustomId = SystemInfo.deviceUniqueIdentifier,//here I have to put the name of the player 
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Error while loging in/creating account");
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnSuccess(LoginResult obj)
    {
        Debug.Log("Sucesfull login/acount created");
    }
    public void SendLeaderBoard(int score)//call this method from the game manager when the game finishes
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName ="TablaPuntajes",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);

    }

    private void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult obj)
    {
        Debug.Log("Succesful leaderboard sent");
    }
    private void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "TablaPuntajes",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnleaderboardGet, OnError);
    }

    private void OnleaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
}
public class User{
    private string userName;
    private int score;
    public int Score { get => score; }
    public string UserName { get => userName; set => userName = value; }
    public User (string userName, int score)
    {
        this.userName = userName;   
        this.score = score;
    }
}
