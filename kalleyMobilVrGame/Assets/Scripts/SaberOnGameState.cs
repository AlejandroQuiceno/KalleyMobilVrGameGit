using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberOnGameState : MonoBehaviour
{
    GameManager gameManager;
    private void Awake()
    {
        gameManager = GameManager.GetInstance();
    }
    private void Start()
    {
        gameManager.OnGameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState curentState)
    {
        if(curentState == GameState.Scoring)
        {
            gameObject.SetActive(false);
        }
    }
}
