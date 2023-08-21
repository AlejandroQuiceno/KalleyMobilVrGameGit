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
    private float timer;
    void Start()
    {
        
    }
    void Update()
    {
        timer += Time.deltaTime;
        if(timer> spawnTimer )
        {
            Transform randomPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];

            GameObject spawnedPrefab = Instantiate(randomPrefab, randomPoint.position, randomPoint.rotation);
            timer =0;
            Rigidbody rb = spawnedPrefab.GetComponent<Rigidbody>();
            Vector3 direction = PlayerTranform.position-randomPoint.position;
            direction.Normalize();
            rb.velocity = direction* velocityIntensity;
        }
    }
}
