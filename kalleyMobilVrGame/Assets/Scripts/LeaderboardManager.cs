using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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
        for (int i=0;i<users.Count;i++) 
        {
            usersUI[i].PopulateUIUser(users[i]);
        } 
    }
}
