using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    GameState currentGameState;
    UIManager uIManager;
    private int canvasIndex = 0;

    //Tutorial fields
    bool firstBoxHit  = false;

    bool nextQuestion = false;
    private List<BoxColor> correctBoxColor = new List<BoxColor>();
    public bool NextQuestion { get => nextQuestion; set => nextQuestion = value; }
    public bool FirstBoxHit { get => firstBoxHit; set => firstBoxHit = value; }
    public GameState CurrentGameState { get => currentGameState; }
    public List<BoxColor> GetCorrectBoxColor { get => correctBoxColor; }


    private SpawnerScript spawner;

    private void Awake()
    {
        spawner = FindObjectOfType<SpawnerScript>();
    }
    void Start()
    {
        uIManager = UIManager.GetInstance();
        if (currentGameState == GameState.Tutorial)
        {
            StartCoroutine(Tutorial());
        }
    }
    public void StartQuestions()
    {
        StartCoroutine("Questioning");
    }
    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(1f);
        uIManager.AnimateCanvasGroupIn(canvasIndex);
        //Verificar que saque la espada
        yield return new WaitForSeconds(5f);
        //OnStartTutorialSpawn?.Invoke();//se empieza a spawnear cubos
        spawner.StartSpawnTutorial();
        correctBoxColor.Add(BoxColor.White);
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

        //Solo cortar los cubos de un color, para efectos practicos los voy a hacer amarillos
        yield return new WaitForSeconds(1.5f);
        BoxColor[] boxColors = { BoxColor.Yellow, BoxColor.Blue };
        spawner.StartSpawning(boxColors, 10);
        correctBoxColor.Clear();
        correctBoxColor.Add(BoxColor.Yellow);
    }
    IEnumerator Questioning()
    {
        yield return new WaitForSeconds(2f);
        uIManager.AnimateCanvasGroupOut(canvasIndex);
        canvasIndex++;
        uIManager.AnimateCanvasGroupIn(canvasIndex);
        yield return new WaitForSeconds(5f);
        uIManager.AnimateCanvasGroupOut(canvasIndex);
        canvasIndex++;
        uIManager.AnimateCanvasGroupIn(canvasIndex);
        yield return new WaitForSeconds(7f);
        currentGameState = GameState.Question;
        uIManager.AnimateCanvasGroupOut(canvasIndex);
        canvasIndex++;
        QuestionManager questionManager = QuestionManager.GetInstance();
        do
        {
            nextQuestion = false;
            uIManager.PopulateQuestion();
            correctBoxColor.Clear();
            correctBoxColor = questionManager.GetQuestion().correctColors;
            uIManager.AnimateCanvasGroupIn(canvasIndex);
            yield return new WaitForSeconds(5);
            spawner.StartSpawning(questionManager.GetQuestion().answerList.Count, 20);
            yield return new WaitUntil(() => nextQuestion == true);
            uIManager.AnimateCanvasGroupOut(canvasIndex);
            questionManager.NextQuestion();
        } while(questionManager.questions.Count >= questionManager.currentQuestionIndex);
    }
}

public enum GameState
{
    Tutorial,Question,Scoring
}