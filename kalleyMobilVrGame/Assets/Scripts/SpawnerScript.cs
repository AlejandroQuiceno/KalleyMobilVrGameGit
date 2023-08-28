using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] Transform PlayerTranform;
    public float velocityIntensity;
    public GameObject[] prefabs;
    public Transform[] SpawnPoints;
    public float spawnTimer = 2;
    float initialSpwanTimer;
    public bool spawn;

    [Range(0, 3)]
    [SerializeField] float randomTargetOffset;
    void Start()
    {
        initialSpwanTimer = 1;
        GameManager gameManager = GameManager.GetInstance();
        gameManager.OnStartSpawning += StartSpawning;
        gameManager.OnStartTutorialSpawn += StartSpawnTutorial;
    }
    public void StartSpawning()
    {
        StartCoroutine(Spawing());
    }
    public void StopSpawning()
    {
        StopAllCoroutines ();
    }
    public void StartSpawnTutorial()
    {
        StartCoroutine(SpawnTutorial());
    }
    IEnumerator Spawing()
    {
        yield return new WaitForSeconds(initialSpwanTimer);
        do
        {
            yield return new WaitForSeconds(spawnTimer);
            spawnTimer = UnityEngine.Random.Range(1.5f, initialSpwanTimer + 1);
            Transform randomPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];

            GameObject spawnedPrefab = Instantiate(randomPrefab, randomPoint.position, randomPoint.rotation);
            Rigidbody rb = spawnedPrefab.GetComponent<Rigidbody>();

            Vector3 direction = PlayerTranform.position - randomPoint.position;
            float randomY = UnityEngine.Random.Range(-randomTargetOffset, randomTargetOffset);
            float randomX = UnityEngine.Random.Range(-randomTargetOffset, randomTargetOffset);
            float randomZ = UnityEngine.Random.Range(-randomTargetOffset, randomTargetOffset);
            Vector3 randomOffset = new Vector3(randomX, randomY, randomZ);
            direction.Normalize();
            randomOffset /= 10;
            rb.velocity = (direction + randomOffset) * velocityIntensity;

        } while(true);
    }
    IEnumerator SpawnTutorial()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTimer+2);
            if (!GameManager.GetInstance().FirstBoxHit)
            {
                GameObject spawnedPrefab = Instantiate(prefabs[0], SpawnPoints[0].position, Quaternion.identity);
                Rigidbody rb = spawnedPrefab.GetComponent<Rigidbody>();
                Vector3 direction = PlayerTranform.position - SpawnPoints[0].position;
                direction.Normalize();
                rb.velocity = (direction) * velocityIntensity;
                if (Time.timeScale == 1) AudioManager.instance.Play("SlowMotionIn");
                Time.timeScale = 0.3f;
            }
            else
            {
                break;
            }
        } 
    }

}

