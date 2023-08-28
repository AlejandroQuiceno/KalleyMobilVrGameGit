using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ObjectSlicer objectSlicer;
    private void Awake()
    {
        objectSlicer = GetComponentInChildren<ObjectSlicer>();
    }
    private void Start()
    {
        objectSlicer.OnBoxHit += BoxSclice;
    }
    public void BoxSclice(int boxCount, float volume)
    {
        if (boxCount == 1)
        {
            GameManager.GetInstance().FirstBoxHit= true;
            AudioManager.instance.Play("SlowMotionOut");
            Time.timeScale = 1f;
        }
    }
}
