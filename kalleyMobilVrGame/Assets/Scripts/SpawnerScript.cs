
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] Transform PlayerTranform;
    public float velocityIntensity;
    public GameObject boxPrefab;
    public Transform[] SpawnPoints;
    public float spawnTimer = 2;
    float initialSpwanTimer;


    [Range(0, 3)]
    [SerializeField] float randomTargetOffset;

    private void Update()
    {
        
    }
    void Start()
    {
        initialSpwanTimer = 1;
        GameManager gameManager = GameManager.GetInstance();
    }
    /// <summary>
    /// This methods spawns colors just from one color
    /// </summary>
    public void StartSpawning(BoxColor[] colors, int boxes)
    {
        StartCoroutine(Spawning(colors,boxes));
    }
    public void StartSpawning(int answers, int boxes)
    {
        BoxColor[] colors = new BoxColor[answers]; 
        if (answers > 1) {
            colors[0] = BoxColor.Yellow;
            colors[1] = BoxColor.Red;
        }
        if (answers > 2)
        {
            colors[2] = BoxColor.Green;
        }
        if (answers > 3)
        {
            colors[2] = BoxColor.Blue;
        }
        StartCoroutine(Spawning(colors, boxes));
    }
    public void StopSpawning()
    {
        StopAllCoroutines ();
    }
    public void StartSpawnTutorial()
    {
        StartCoroutine(SpawnTutorial());
    }

    IEnumerator Spawning(BoxColor[] colors,int boxesToSpawn)
    {
        int boxCounter=0;
        yield return new WaitForSeconds(initialSpwanTimer);
        QuestionManager questionManager = QuestionManager.GetInstance();
        GameManager gameManager = GameManager.GetInstance();
        List <BoxColor> randomColorsList = new List<BoxColor>();
        List<BoxColor> incorrectColots = new List<BoxColor>();
        for(int i = 0; i < boxesToSpawn; i++)
        {
            if (i <= 5 && gameManager.CurrentGameState == GameState.Question)
            {
                int radomCorrectColorIndex = Random.Range(0, questionManager.GetQuestion().correctColors.Count);
                randomColorsList.Add(questionManager.GetQuestion().correctColors[radomCorrectColorIndex]);
            }
            else
            {
                int randomIndex = Random.Range(0, colors.Length);
                randomColorsList.Add(colors[randomIndex]);
            }
        }
        ListRandomizer.RandomizeList(randomColorsList);
        int colorindex = 0;
        do
        {
            yield return new WaitForSeconds(Random.Range(0.3f,spawnTimer));
            SpawnCube(randomColorsList[colorindex]);
            colorindex++;
            Debug.Log("Spawned a cube"+" boxCounter"+boxCounter);
            boxCounter++;
        } while(boxCounter != boxesToSpawn);
        if (gameManager.CurrentGameState == GameState.Tutorial) gameManager.StartQuestions();
        if(gameManager.CurrentGameState == GameState.Question) gameManager.NextQuestion = true;
    }
    public void SpawnCube(BoxColor color)
    {
        spawnTimer = Random.Range(1.5f, initialSpwanTimer + 1);
        Transform randomPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        GameObject spawnedPrefab = Instantiate(boxPrefab, randomPoint.position, randomPoint.rotation);
        //change the color
        BoxController boxController = spawnedPrefab.GetComponent<BoxController>();
        boxController.SetBoxColor(color);

        Rigidbody rb = spawnedPrefab.GetComponent<Rigidbody>();
        Vector3 direction = PlayerTranform.position - randomPoint.position;
        float randomY = Random.Range(-randomTargetOffset, randomTargetOffset);
        float randomX = Random.Range(-randomTargetOffset, randomTargetOffset);
        float randomZ = Random.Range(-randomTargetOffset, randomTargetOffset);
        Vector3 randomOffset = new Vector3(randomX, randomY, randomZ);
        direction.Normalize();
        randomOffset /= 10;
        rb.velocity = (direction + randomOffset) * velocityIntensity;
    }
    IEnumerator SpawnTutorial()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTimer+2);
            if (!GameManager.GetInstance().FirstBoxHit)
            {
                GameObject spawnedPrefab = Instantiate(boxPrefab, SpawnPoints[0].position, Quaternion.identity);
                Rigidbody rb = spawnedPrefab.GetComponent<Rigidbody>();
                BoxController boxController = spawnedPrefab.GetComponent<BoxController>();
                boxController.SetBoxColor(BoxColor.White);
                Vector3 direction = PlayerTranform.position - SpawnPoints[0].position;
                direction.Normalize();
                rb.velocity = (direction) * velocityIntensity;
                if (Time.timeScale == 1) AudioManager.instance.Play("SlowMotionIn");
                Time.timeScale = 0.45f;
            }
            else
            {
                break;
            }
        } 
    }

}

