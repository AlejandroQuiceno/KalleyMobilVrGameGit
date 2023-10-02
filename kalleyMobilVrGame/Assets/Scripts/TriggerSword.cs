using ArmnomadsGames;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerSword : MonoBehaviour
{
    [SerializeField] private InputAction SwordAction;
    LaserSword laserSword;
    bool enabled = false;
    GameManager gameManager;
    private void Awake()
    {
        laserSword= GetComponent<LaserSword>();
        gameManager = GameManager.GetInstance();
    }
    private void OnEnable()
    {
        SwordAction.performed += OnJumpPerformed;
        SwordAction.Enable();
    }

    private void OnDisable()
    {
        SwordAction.Disable();
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (context.performed && !enabled)
        {
            laserSword.Enable();
            enabled = true;
            gameManager.SwordTriggered = true;
        }
        else if (context.performed && enabled)
        {
            laserSword.Disable();
            enabled = false;
        }
    }
}
