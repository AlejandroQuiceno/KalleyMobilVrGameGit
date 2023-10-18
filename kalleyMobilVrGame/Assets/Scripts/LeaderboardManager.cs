using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public List<User> users;
    
    [SerializeField] UserScoreController[] usersUI;
    PlayfabManager playfabManager;
    private void Awake()
    {
        usersUI = GetComponentsInChildren<UserScoreController>();
    }
    public void PopulateUsersUI(List<User> users)
    {
        this.users = users;
        var sortedUsers = users.OrderByDescending(user => user.Score).ToList();
        for (int i=0;i< sortedUsers.Count;i++) 
        {
            usersUI[i].PopulateUIUser(sortedUsers[i]);
        } 
    }
}
