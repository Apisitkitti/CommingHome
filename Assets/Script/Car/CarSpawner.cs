using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab;
    //public Transform spawnPoint;
    public List<Transform> spawnPoint;
    public TrafficLightController trafficLightController;
    public float spawnInterval = 3f;

    private float timer;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (trafficLightController.IsGreen() && timer >= spawnInterval)
        {
            SpawnCar();
            timer = 0f;
        }
    }

    void SpawnCar()
    {
        int RandomSpawn = Random.Range(0,spawnPoint.Count);
        Transform WhereSpawn = spawnPoint[RandomSpawn];
        Instantiate(carPrefab, WhereSpawn.position, WhereSpawn.rotation);
    }
}