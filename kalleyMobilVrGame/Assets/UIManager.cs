using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    //Tutorial fields
    [SerializeField] List<UIAnimationScript> uIGroups;
    public void AnimateCanvasGroupIn(int canvasIndex)
    {
        uIGroups[canvasIndex].AnimateIn();
    }
    public void AnimateCanvasGroupOut(int canvasIndex)
    {
        uIGroups[canvasIndex].AnimateOut();
    }
}
