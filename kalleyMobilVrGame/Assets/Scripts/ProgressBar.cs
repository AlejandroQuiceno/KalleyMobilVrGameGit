using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Image fillImage;
    private CanvasGroup canvasGroup;
    float startFilling;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void IncreaseFill()
    {
        startFilling = fillImage.fillAmount;
        fillImage.DOFillAmount(startFilling + 0.25f,1);
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
