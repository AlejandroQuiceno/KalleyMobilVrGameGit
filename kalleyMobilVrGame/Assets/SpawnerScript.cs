using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] Transform PlayerTranform;
    public float velocityIntensity;
    public GameObject[] prefabs;
    public Transform[] SpawnPoints;
    public float spawnTimer = 2;
    float initialSpwanTimer;
    private float timer;
    [Range(0, 3)]
    [SerializeField] float randomTargetOffset;
    void Start()
    {
        initialSpwanTimer = 2;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if(timer> spawnTimer )
        {
            spawnTimer = UnityEngine.Random.Range(1.5f, initialSpwanTimer + 1);
            Transform randomPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];

            GameObject spawnedPrefab = Instantiate(randomPrefab, randomPoint.position, randomPoint.rotation);
            timer =0;
            Rigidbody rb = spawnedPrefab.GetComponent<Rigidbody>();

            Vector3 direction = PlayerTranform.position-randomPoint.position;
            float randomY = UnityEngine.Random.Range(-randomTargetOffset, randomTargetOffset);
            float randomX = UnityEngine.Random.Range(-randomTargetOffset, randomTargetOffset);
            float randomZ = UnityEngine.Random.Range(-randomTargetOffset, randomTargetOffset);
            Vector3 randomOffset = new Vector3(randomX, randomY, randomZ);
            direction.Normalize();
            randomOffset /= 10;
            rb.velocity = (direction+ randomOffset) * velocityIntensity;
        }
    }
}
