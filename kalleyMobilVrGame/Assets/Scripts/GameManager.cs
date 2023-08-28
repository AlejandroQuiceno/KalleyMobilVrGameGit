using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    GameState currentGameState;
    UIManager uIManager;
    private int canvasIndex = 0;

    //Tutorial fields
    bool firstBoxHit  = false;

    public bool FirstBoxHit { get => firstBoxHit; set => firstBoxHit = value; }

    public delegate void SpawnTutorial();
    public event SpawnTutorial OnStartTutorialSpawn;

    public delegate void startSpwaning();
    public event startSpwaning OnStartSpawning;
    void Start()
    {
        uIManager = UIManager.GetInstance();
        if (currentGameState == GameState.Tutorial)
        {
            StartCoroutine(Tutorial());
        }
    }
    public void Update()
    {
        Debug.Log(canvasIndex);
    }
    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(1f);
        uIManager.AnimateCanvasGroupIn(canvasIndex);
        //Verificar que saque la espada
        yield return new WaitForSeconds(5f);
        OnStartTutorialSpawn?.Invoke();//se empieza a spawnear cubos
        uIManager.AnimateCanvasGroupOut(canvasIndex);
        canvasIndex++;
        uIManager.AnimateCanvasGroupIn(canvasIndex);
        do
        {
            yield return null;
        } while (!firstBoxHit);
        yield return new WaitForSeconds(0.5f);
        uIManager.AnimateCanvasGroupOut(canvasIndex);
        canvasIndex++;
        uIManager.AnimateCanvasGroupIn(canvasIndex);

        //pregunta con respuestas
    }
}
public enum GameState
{
    Tutorial,Question,Scoring
}
