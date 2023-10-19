using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    //Tutorial fields
    [SerializeField] List<UIAnimationScript> CanvasGroups;
    [SerializeField] Camera mainCamera;
    //score fields
    [Header("Feedback Fields")]
    [SerializeField] GameObject scoreRightText;
    [SerializeField] GameObject scoreRightImage;
    [SerializeField] GameObject scoreWrongImage;
    [SerializeField] Canvas canvasfeedback;
    [SerializeField] Color scoreColor;
    [SerializeField] Color scoreFadeColor;
    [SerializeField] float scoreDuration;
    //Question title
    [Header("Question Fields")]
    [SerializeField] TMP_Text questiontitle;
    [SerializeField] List<TMP_Text> answers;

    private void Awake()
    {
        mainCamera = FindObjectOfType<Camera>();
    }
    public void AnimateCanvasGroupIn(int canvasIndex)
    {
        CanvasGroups[canvasIndex].gameObject.SetActive(true);
        CanvasGroups[canvasIndex].AnimateIn();
    }
    public void AnimateCanvasGroupOut(int canvasIndex)
    {
        CanvasGroups[canvasIndex].AnimateOut();
        CanvasGroups[canvasIndex].gameObject.SetActive(false);
    }
    public void InstantiateScore(float volume, Vector3 worldPosition,BoxColor boxColor)
    {
        //Check if the box hit was correct
        bool corect = CheckBoxColorisCorrect(boxColor);
        GameObject instance;
        if (corect && GameManager.GetInstance().CurrentGameState == GameState.Question)
        {
            instance = Instantiate(scoreRightText, worldPosition, Quaternion.identity, canvasfeedback.transform);
            RectTransform rectTransform = instance.GetComponent<RectTransform>();
            instance.TryGetComponent<TMP_Text>(out TMP_Text scoreText);
            if (volume <= 0.7 && volume >= 0.3)
            {
                scoreText.text = 300f.ToString();
                ScoreManager.GetInstance().AddScore(300);
            }
            else
            {
                scoreText.text = 150f.ToString();
                ScoreManager.GetInstance().AddScore(150);
            }
            Debug.Log("volume: "+volume+",score: "+scoreText.text);
            TweenScore(scoreText, rectTransform);
        }
        else if (corect && GameManager.GetInstance().CurrentGameState == GameState.Tutorial)
        {
            instance = Instantiate(scoreRightImage, worldPosition, Quaternion.identity, canvasfeedback.transform);
            RectTransform rectTransform = instance.GetComponent<RectTransform>();
            instance.TryGetComponent<Image>(out Image image);
            TweenScore(image, rectTransform);
        }
        else
        {
            instance = Instantiate(scoreWrongImage, worldPosition, Quaternion.identity, canvasfeedback.transform);
            RectTransform rectTransform = instance.GetComponent<RectTransform>();
            instance.TryGetComponent<Image>(out Image image);
            TweenScore(image, rectTransform);
            //AudioManager.instance.Play("BoxHitWrong");
        }
    }
    private void TweenScore(TMP_Text scoreText, RectTransform rectTransform)
    {
        rectTransform.DOLookAt(mainCamera.transform.position,0);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(scoreText.DOColor(scoreColor, 0.3f));
        mySequence.Join(rectTransform.DOPunchScale(new Vector3(0.4f, 0.4f, 0), scoreDuration / 3f, 1, 1));
        mySequence.Join(rectTransform.DOMoveY((rectTransform.anchoredPosition.y + 10) / 100, scoreDuration).SetEase(Ease.OutQuad));
        mySequence.Append(scoreText.DOColor(scoreFadeColor, 0.5f));
        mySequence.Join(rectTransform.DOMoveY((rectTransform.anchoredPosition.y + 1)/100, 0.5f));
        mySequence.Join(rectTransform.DOScale(0.85f,0.5f));
        mySequence.Play();
    }
    private void TweenScore(Image image, RectTransform rectTransform)
    {
        rectTransform.DOLookAt(mainCamera.transform.position, 0);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Join(rectTransform.DOPunchScale(new Vector3(0.4f, 0.4f, 0), scoreDuration / 3f, 1, 1));
        mySequence.Join(rectTransform.DOMoveY((rectTransform.anchoredPosition.y + 10)/100, scoreDuration).SetEase(Ease.OutQuad));
        mySequence.Append(image.DOColor(scoreFadeColor, 0.5f));
        mySequence.Join(rectTransform.DOMoveY((rectTransform.anchoredPosition.y + 1f) / 100, 0.5f));
        mySequence.Join(rectTransform.DOScale(0.85f, 0.5f));
        mySequence.Play();
    }
    private bool CheckBoxColorisCorrect(BoxColor boxColor)
    {
        if(GameManager.GetInstance().GetCorrectBoxColor.Contains(boxColor)) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void PopulateQuestion()
    {
        Question question = QuestionManager.GetInstance().GetQuestion();
        questiontitle.text = question.question;
        for(int i =0;i<answers.Count;i++)
        {
            if (question.answerList.Count > i) 
            {
                answers[i].gameObject.SetActive(true);
                answers[i].text = question.answerList[i];

            } 
            else
            {
                answers[i].gameObject.SetActive(false);
            }
        }
    }
}
