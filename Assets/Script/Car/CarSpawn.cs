using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class CarSpawn : MonoBehaviour
{
  [Header("Spawn")]
  [SerializeField] List<Transform> carSpawner;
  // [SerializeField] List<GameObject> spawnerGameObject;
  [SerializeField] List<GameObject> carPrefab;
  [SerializeField] List<GameObject> carThatSpawnOnThemap;


  [Header("ControlTrafffic start with one ")]
  [SerializeField] TrafficControl trafficControl;
  [SerializeField] int controlTrafficSpawn;
  int spawnerNumber = 0;

  #region ForSetSpawnCar
  [Header("CarSpawnSetting")]
  [SerializeField] float spawnTimeSetting = 0f;
  float spawnCountDown;
  int randomCarSpawn;
  int start, endCar;
  #endregion

  void Start()
  {
    for (int index = 0; index < trafficControl.carSpawnEnable.Count; index++)
    {
      trafficControl.carSpawnEnable[index] = false;
    }
    start = 0;
    endCar = carPrefab.Count;
  }

  void Update()
  {

    randomCarSpawn = randomNumber();
    Debug.Log(randomNumber());

    if (trafficControl.carSpawnEnable[controlTrafficSpawn - 1])
    {
      if (spawnCountDown > 0)
      {
        spawnCountDown -= Time.deltaTime;
      }
      else if (spawnCountDown <= 0)
      {
        checkWhereToSpawn();
        spawnCountDown = spawnTimeSetting;
      }

    }
  }



  void checkWhereToSpawn()
  {
    if (spawnerNumber >= carSpawner.Count)
    {
      spawnerNumber = 0;
    }
    carSpawnAndAddList(spawnerNumber);
    spawnerNumber++;
  }






  void carSpawnAndAddList(int spawnerNumber)
  {

    GameObject carSpawn = Instantiate(carPrefab[randomNumber()], carSpawner[spawnerNumber].position, carSpawner[spawnerNumber].rotation);
    carThatSpawnOnThemap.Add(carSpawn);
  }

  int randomNumber()
  {
    return UnityEngine.Random.Range(start, endCar);
  }
}

