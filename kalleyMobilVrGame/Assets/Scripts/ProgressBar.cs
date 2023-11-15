using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Image fillImage;
    private CanvasGroup canvasGroup;
    public float startFilling = 0.059f;
    public float verticalDistance = 1;
    private float originalPosition;
    RectTransform rect;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        fillImage.fillAmount = 0;
        originalPosition = rect.localPosition.y;
    }
    public void InicitialFill()
    {
        Move();
        AudioManager.instance.Play("load");
        fillImage.DOFillAmount(startFilling, 1.3f);
    }
    private IEnumerator increaseFill()
    {
        Move();
        startFilling = fillImage.fillAmount;
        yield return new WaitForSeconds(1);
        AudioManager.instance.Play("load");
        fillImage.DOFillAmount(startFilling + 0.236f, 1.3f);
    }
    public void IncreaseFill()
    {
        StartCoroutine(increaseFill());
    }
    private void Move()
    {
        rect.DOLocalMoveY(rect.localPosition.y - verticalDistance, 1f);
        rect.DOScale(3f, 1f);
        rect.DOLocalMoveY(originalPosition, 1f).SetDelay(3);
        rect.DOScale(1.7f, 1f).SetDelay(3);
    }
    public void Enable(bool enable)
    {
        if (enable)
        {
            canvasGroup.DOFade(1, 1);
        }
        else
        {
            canvasGroup.DOFade(0, 1);
        }
    }
}
