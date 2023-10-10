using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.SocialPlatforms.Impl;
using System;
public class PlayfabManager : MonoBehaviour
{
    public void SignUp(string name)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Username = name, // Set the player's name as the username
            Password = "YourPassword", // Set a password for the new account
            RequireBothUsernameAndEmail = false // You can adjust this as needed
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnSuccess, OnError);
    }

    private void OnSuccess(RegisterPlayFabUserResult obj)
    {
        Debug.Log("user logged in");
        SendLeaderBoard(ScoreManager.GetInstance().Score);
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Error while loging in/creating account");
        Debug.Log(error.GenerateErrorReport());
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
        GameManager.GetInstance().NameFieldEnter = true;
    }
    public void GetLeaderBoard()
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
        List<User> users = new List<User>();
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
            users.Add(new User(item.PlayFabId, item.StatValue));
        }
        FindObjectOfType<LeaderboardManager>().PopulateUsersUI(users);
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
