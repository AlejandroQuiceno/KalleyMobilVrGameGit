using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAnimatorOnState : MonoBehaviour
{
    GameManager gameManager;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameManager.GetInstance();
        gameManager.OnGameStateChanged += OnGameStateChanged;
    }

    public void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Scoring)
        {
            animator.enabled = true;
            transform.localRotation = Quaternion.Euler(0, 0, -90);
            transform.localPosition = new Vector3(-0.001f, 0.001f, -0.035f);
        }
    }
}
