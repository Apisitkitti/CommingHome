using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawn : MonoBehaviour
{
  [Header("Spawn")]
  [SerializeField] List<Transform> carSpawner;
  [SerializeField] GameObject carPrefab;


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
      currentTime -= 1 * Time.deltaTime;
      return false;
    }
  }

  void carSpawn()
  {
    for (int spawnerNumber = 0; spawnerNumber < carSpawner.Count; spawnerNumber++)
    {
      var plsSpawn = carSpawnTime() ? Instantiate(carPrefab, carSpawner[spawnerNumber].position, carSpawner[spawnerNumber].rotation) : null;
    }

  }
  float resetTime()
  {
    currentTime = spawnTimeSetting;
    return currentTime;
  }
}
