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
    LeaderboardManager leaderboardManager;
    User currentUser;
    private void Awake()
    {
        leaderboardManager = FindObjectOfType<LeaderboardManager>();
    }
    public void SignUp(string name)
    {
        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = name, // Set the player's name as the username
            Username = name,
            Password = "YourPassword", // Set a password for the new account
            RequireBothUsernameAndEmail = false // You can adjust this as needed

        };
        leaderboardManager.currentUser = new User(name, ScoreManager.GetInstance().Score);
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
        GetLeaderBoard();
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
        if (result.Leaderboard == null || result.Leaderboard.Count == 0)
        {
            Debug.Log("No users found in the leaderboard.");
            GameManager.GetInstance().NameFieldEnter = true;
            FindObjectOfType<LeaderboardManager>().DiplayCurrentUser();
        }
        else
        {
            List<User> users = new List<User>();

            // Iterate through the leaderboard items
            foreach (var item in result.Leaderboard)
            {
                // Fetch the player's profile using the PlayFabId
                PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest
                {
                    PlayFabId = item.PlayFabId,
                    ProfileConstraints = new PlayerProfileViewConstraints
                    {
                        ShowDisplayName = true // This ensures that the username (display name) is retrieved
                    }
                }, (profileResult) =>
                {
                    if (profileResult.PlayerProfile != null)
                    {
                        string username = profileResult.PlayerProfile.DisplayName;
                        User newuser = new User(username, item.StatValue);
                        users.Add(newuser);
                        FindObjectOfType<LeaderboardManager>().PopulateUsersUI(newuser);
                    }
                    else
                    {
                        Debug.LogError("Failed to get player profile");
                    }
                }, null);
            }
            GameManager.GetInstance().NameFieldEnter = true;
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
