using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedBoxesController : MonoBehaviour
{
    private ObjectSlicer objectSlicer;
    private int boxesSliced;

    public int BoxesSliced { get => boxesSliced; }

    private void Awake()
    {
        objectSlicer = FindObjectOfType<ObjectSlicer>();
    }
    private void Start()
    {
        objectSlicer.OnBoxHit += CheckScore;
    }
    public void CheckScore(float hullVolume,Vector3 hullPosition,BoxColor boxColor)
    {
        boxesSliced++;
        if (boxesSliced == 1)
        {
            GameManager.GetInstance().FirstBoxHit= true;
            AudioManager.instance.Play("SlowMotionOut");
            Time.timeScale = 1f;
        }
        UIManager.GetInstance().InstantiateScore(hullVolume, hullPosition, boxColor);
    }
}
