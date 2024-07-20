using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class CarSpawn : MonoBehaviour
{
  [Header("Spawn")]
  [SerializeField] List<Transform> carSpawner;
  [SerializeField] GameObject carPrefab;
  [SerializeField] List<GameObject> carThatSpawn;
  int spawnerNumber = 0;

  #region ForSetSpawnCar
  [Header("CarSpawnSetting")]
  [SerializeField] float spawnTimeSetting = 0f;
  [SerializeField] float currentTime = 0f;
  float timeToSpawn = 0f;
  #endregion


  void Start()
  {
    resetTime();
  }
  void Update()
  {
    carSpawnTime();
    carSpawn();
  }
  bool carSpawnTime()
  {
    if (currentTime <= timeToSpawn)
    {
      resetTime();
      return true;
    }
    else
    {
      currentTime -= Time.deltaTime;
      return false;
    }
  }

  void carSpawn()
  {
    if (carSpawnTime())
    {
      carForCheck(spawnerNumber);
      spawnerNumber++;
    }
    if (spawnerNumber > carSpawner.Count - 1)
    {
      spawnerNumber = 0;
    }

  }
  float resetTime()
  {
    currentTime = spawnTimeSetting;
    return currentTime;
  }
  void carForCheck(int spawnerNumber)
  {
    GameObject carSpawn = Instantiate(carPrefab, carSpawner[spawnerNumber].position, carSpawner[spawnerNumber].rotation);
    carThatSpawn.Add(carSpawn);
  }
}

