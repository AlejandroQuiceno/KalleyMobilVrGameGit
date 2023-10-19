using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class UIAnimationScript : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    
    public float duration;
    public Ease ease;
    public float positionOffset = 600;
    //public RectTransform[] rectTransforms;
    [SerializeField] AnimationDirection animationDirectionIn = AnimationDirection.Up;
    [SerializeField] AnimationDirection animationDirectionOut = AnimationDirection.Down;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        //rectTransforms = GetComponentsInChildren<RectTransform>();
    }
    private void Start()
    {
        canvasGroup.alpha = 0;
    }
    public void AnimateIn()
    {
        RectTransform groupRect = canvasGroup.GetComponent<RectTransform>();
        Vector3 scale = groupRect.localScale;
        groupRect.localScale = scale * 0.2f;
        canvasGroup.alpha = 0;
        Vector2 anchoredPosition = groupRect.anchoredPosition;
        if (animationDirectionIn == AnimationDirection.Left)
        {
            anchoredPosition.x = groupRect.anchoredPosition.x - positionOffset;
            anchoredPosition.y = 0;
        }
        else if (animationDirectionIn == AnimationDirection.Right)
        {
            anchoredPosition.x = groupRect.anchoredPosition.x + positionOffset;
            anchoredPosition.y = 0;
        }
        else if (animationDirectionIn == AnimationDirection.Up)
        {
            anchoredPosition.y = groupRect.anchoredPosition.y + positionOffset;
            anchoredPosition.x = 0;
        }
        else if (animationDirectionIn == AnimationDirection.Down)
        {
            anchoredPosition.y = groupRect.anchoredPosition.y + positionOffset;
            anchoredPosition.x = 0;
        }
        groupRect.anchoredPosition = anchoredPosition;
        //AnimateChildrenIn(anchoredPosition);
        groupRect.DOLocalMove( Vector2.zero, duration+0.3f).SetEase(ease);
        groupRect.DOScale(scale, duration).SetEase(ease);
        canvasGroup.DOFade(1, duration - 0.5f).SetEase(ease);
        AudioManager.instance.Play("UIwhoosh");
    }
    public void AnimateOut()
    {
        RectTransform groupRect = canvasGroup.GetComponent<RectTransform>();
        Vector2 anchoredPosition = groupRect.anchoredPosition;
        Vector3 scale = groupRect.localScale;
        if (animationDirectionOut == AnimationDirection.Left)
        {
            anchoredPosition.x = groupRect.anchoredPosition.x + positionOffset;
            anchoredPosition.y = 0;
        }
        else if (animationDirectionOut == AnimationDirection.Right)
        {
            anchoredPosition.x = groupRect.anchoredPosition.x - positionOffset;
            anchoredPosition.y = 0;
        }
        else if (animationDirectionOut == AnimationDirection.Up)
        {
            anchoredPosition.y = groupRect.anchoredPosition.y + positionOffset;
            anchoredPosition.x = 0;
        }
        else if (animationDirectionOut == AnimationDirection.Down)
        {
            anchoredPosition.y = groupRect.anchoredPosition.y - positionOffset;
            anchoredPosition.x = 0;
        }
        //AnimateChildrenOut(anchoredPosition);
        groupRect.DOLocalMove(anchoredPosition, duration + 0.3f).SetEase(ease);
        groupRect.DOScale(scale*0.2f, duration ).SetEase(ease);
        canvasGroup.DOFade(0, duration - 0.5f).SetEase(ease);
        AudioManager.instance.Play("UIwhoosh");
    }
    /*
    private  void AnimateChildrenIn(Vector2 anchoredPosition)
    {
        RectTransform[] tempAnchorPos = rectTransforms;
        for (int i =1;i<rectTransforms.Length;i++)
        {
            rectTransforms[i].anchoredPosition.Set(rectTransforms[i].anchoredPosition.x + anchoredPosition.x, rectTransforms[i].anchoredPosition.y + anchoredPosition.y);
            rectTransforms[i].DOLocalMove(tempAnchorPos[i].anchoredPosition, duration + (i + 2) / 10);
            Debug.Log("Position: " + tempAnchorPos[i].anchoredPosition + " Duration: " + duration + (i + 2) / 10);
        }
    }
    private void AnimateChildrenOut(Vector2 anchoredPosition)
    {
        RectTransform[] tempAnchorPos = rectTransforms;
        for (int i = 1; i < rectTransforms.Length; i++)
        {
            rectTransforms[i].DOLocalMove(tempAnchorPos[i].anchoredPosition + anchoredPosition, duration + (i + 2) / 10);
        }
    }
    */
}
public enum AnimationDirection
{
    Left,
    Right,
    Up,
    Down
}