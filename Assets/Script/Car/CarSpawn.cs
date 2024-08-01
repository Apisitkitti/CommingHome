using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class CarSpawn : MonoBehaviour
{
  [Header("Spawn")]
  [SerializeField] List<Transform> carSpawner;
  [SerializeField] List<GameObject> spawnerGameObject;
  [SerializeField] GameObject carPrefab;
  [SerializeField] List<GameObject> carThatSpawn;
  int spawnerNumber = 0;

  #region ForSetSpawnCar
  [Header("CarSpawnSetting")]
  [SerializeField] float spawnTimeSetting = 0f;
  public List<bool> isSpawn;

  #endregion
  void Awake()
  {
    foreach (Transform spawnerTransform in carSpawner)
    {
      spawnerGameObject.Add(spawnerTransform.gameObject);
    }
  }
  void Start()
  {
    spawnObjectSet(false);
  }
  void Update()
  {
    checkWhereToSpawn();
  }

  public void checkWhereToSpawn()
  {
    foreach (bool isSpawnRequest in isSpawn)
    {
      if (isSpawnRequest)
      {
        spawnerGameObject[spawnerNumber].SetActive(true);
        StartCoroutine(carSpawn());
        spawnerNumber++;
      }
      if (spawnerNumber >= carSpawner.Count)
      {
        spawnerNumber = 0;
      }
    }
  }
  IEnumerator carSpawn()
  {
    carSpawnAndAddList(spawnerNumber);
    yield return new WaitForSeconds(spawnTimeSetting);
  }
  void carSpawnAndAddList(int spawnerNumber)
  {
    GameObject carSpawn = Instantiate(carPrefab, carSpawner[spawnerNumber].position, carSpawner[spawnerNumber].rotation);
    carThatSpawn.Add(carSpawn);
  }

  void spawnObjectSet(bool setBool)
  {
    for (int spawnCheck = 0; spawnCheck < isSpawn.Count; spawnCheck++)
    {
      isSpawn[spawnCheck] = setBool;
      spawnerGameObject[spawnCheck].SetActive(setBool);
    }
  }
}

