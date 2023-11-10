using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UserScoreController : MonoBehaviour
{
    [SerializeField] TMP_Text name;
    [SerializeField] TMP_Text score;
    public CanvasGroup canvasGroup;
    private User user;
    public void PopulateUIUser(User user)
    {
        this.user = user;
        name.text = user.UserName;
        score.text = user.Score.ToString();
        canvasGroup.alpha = 1;
    }
}
