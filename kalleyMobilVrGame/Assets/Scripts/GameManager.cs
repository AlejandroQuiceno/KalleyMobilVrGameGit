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
    private bool firstBoxHit  = false;
    private bool swordTriggered = false;
    private bool nextQuestion = false;
    private bool nameFieldEnter = false;

    private List<BoxColor> correctBoxColor = new List<BoxColor>();
    public bool NextQuestion { get => nextQuestion; set => nextQuestion = value; }
    public bool FirstBoxHit { get => firstBoxHit; set => firstBoxHit = value; }
    public GameState CurrentGameState { get => currentGameState; }
    public List<BoxColor> GetCorrectBoxColor { get => correctBoxColor; }
    public bool SwordTriggered { get => swordTriggered; set => swordTriggered = value; }
    public bool NameFieldEnter { get => nameFieldEnter; set => nameFieldEnter = value; }

    public int quiestionAmount;
    private SpawnerScript spawner;

    private ProgressBar progressBar;

    private FeedbackAnimation feedbackAnimation;

    public delegate void state(GameState curentState);
    public event state OnGameStateChanged;

    private void Awake()
    {
        spawner = FindObjectOfType<SpawnerScript>();
        progressBar = FindObjectOfType<ProgressBar>();
        feedbackAnimation = FindObjectOfType<FeedbackAnimation>();
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
        yield return new WaitUntil(() => swordTriggered == true);
        yield return new WaitForSeconds(1f);
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
        OnGameStateChanged?.Invoke(currentGameState);
        uIManager.AnimateCanvasGroupOut(canvasIndex);
        progressBar.Enable(true);
        progressBar.InicitialFill();
        yield return new WaitForSeconds(5f);
        canvasIndex++;
        QuestionManager questionManager = QuestionManager.GetInstance();
        do
        {
            nextQuestion = false;
            uIManager.PopulateQuestion();
            correctBoxColor.Clear();
            correctBoxColor = questionManager.GetQuestion().correctColors;
            uIManager.AnimateCanvasGroupIn(canvasIndex);
            yield return new WaitForSeconds(8);
            spawner.StartSpawning(questionManager.GetQuestion().answerList.Count, 20);
            yield return new WaitUntil(() => nextQuestion == true);
            yield return new WaitForSeconds(2f);
            feedbackAnimation.ShowCorrectAnswers();
            yield return new WaitForSeconds(6f);
            uIManager.AnimateCanvasGroupOut(canvasIndex);
            if(questionManager.currentQuestionIndex+1 < quiestionAmount)
            {
                progressBar.IncreaseFill();
            }
            else
            {
                progressBar.Enable(false);
            }
            yield return new WaitForSeconds(5f);
            questionManager.NextQuestion();
        } while (questionManager.currentQuestionIndex < quiestionAmount);//questionManager.questions.Count >= questionManager.currentQuestionIndex
        uIManager.AnimateCanvasGroupOut(canvasIndex);
        progressBar.Enable(false);
        yield return new WaitForSeconds(2f);
        currentGameState = GameState.Scoring;
        OnGameStateChanged?.Invoke(currentGameState);
        canvasIndex++;
        uIManager.AnimateCanvasGroupIn(canvasIndex);
        yield return new WaitForSeconds(5);
        uIManager.AnimateCanvasGroupOut(canvasIndex);
        canvasIndex++;
        uIManager.AnimateCanvasGroupIn(canvasIndex);

        yield return new WaitUntil(() => nameFieldEnter == true);
        uIManager.AnimateCanvasGroupOut(canvasIndex);
        canvasIndex++;
        uIManager.AnimateCanvasGroupIn(canvasIndex);
    }
}

public enum GameState
{
    Tutorial,Question,Scoring
}
