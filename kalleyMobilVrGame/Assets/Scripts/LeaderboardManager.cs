using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public List<User> users;
    bool called;
    [SerializeField] UserScoreController[] usersUI;
    PlayfabManager playfabManager;
    public User currentUser;
    private void Awake()
    {
        users = new List<User>();
    }
    public void PopulateUsersUI(User user)
    {
        users.Add(user);
        if (users.Count == 10)
        {
            DisplayUsers();
        }
        else
        {
            Invoke("DisplayUsers",1);
        }
    }
    private void DisplayUsers()
    {
        if (currentUser != null)
        {
            users.Add(currentUser);
            currentUser = null;
        }
        if (!called)
        {
            users = users.OrderByDescending(u => u.Score).ToList();
            called = true;
            for (int i = 0; i < Mathf.Min(users.Count, usersUI.Length); i++)
            {
                usersUI[i].PopulateUIUser(users[i]);
            }
        }
    }
}
