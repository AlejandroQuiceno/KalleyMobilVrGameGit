using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FeedbackAnimation : MonoBehaviour
{
    [SerializeField] Image[] backgroundImages;
    List<int> correctAnserindexes = new List<int>();
    public void ShowCorrectAnswers()
    {
        correctAnserindexes = GetCorrectAsnswerIndexes();
        for(int i = 0; i< correctAnserindexes.Count; i++)
        {
            Debug.Log("correct index"+correctAnserindexes[i]);
            Sequence sequence = DOTween.Sequence(); 
            sequence.Append(backgroundImages[correctAnserindexes[i]].DOFade(0, 0.1f));
            sequence.Append(backgroundImages[correctAnserindexes[i]].DOFade(0.3f, 0.1f));
            sequence.Append(backgroundImages[correctAnserindexes[i]].DOFade(0, 0.1f).SetDelay(0.15f));
            sequence.Append(backgroundImages[correctAnserindexes[i]].DOFade(0.3f, 0.1f));
            sequence.Append(backgroundImages[correctAnserindexes[i]].DOFade(0, 0.1f).SetDelay(0.15f));
            sequence.Append(backgroundImages[correctAnserindexes[i]].DOFade(0.3f, 0.1f));
            sequence.Append(backgroundImages[correctAnserindexes[i]].DOFade(0, 0.1f).SetDelay(6f));
        }
        AudioManager.instance.Play("CorrectAnswer");
    }
    private List<int> GetCorrectAsnswerIndexes()
    {
        var correctColors = GameManager.GetInstance().GetCorrectBoxColor;
        List<int> correctIndexes = new List<int>();
        if (correctColors.Contains(BoxColor.Yellow))
        {
            correctIndexes.Add(0);
        }
        if (correctColors.Contains(BoxColor.Red))
        {
            correctIndexes.Add(1);
        }
        if (correctColors.Contains(BoxColor.Green))
        {
            correctIndexes.Add(2);
        }
        if (correctColors.Contains(BoxColor.Blue))
        {
            correctIndexes.Add(3);
        }
        return correctIndexes;
    }

}
