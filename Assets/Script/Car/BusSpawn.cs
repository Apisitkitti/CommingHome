using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BusSpawn : MonoBehaviour
{
  [SerializeField] GameObject busPrefab;
  [SerializeField] Transform busSpawn;
  [SerializeField] float timeSpawn;
  private float currentTime;
  private float endTime = 0;
  bool hasSpawn = true;
  void Start()
  {
    currentTime = timeSpawn;
  }
  void Update()
  {
    setSpawBus();
    busSpawner();
  }
  void busSpawner()
  {
    if (setSpawBus() && hasSpawn)
    {
      var busSpawnCar = Instantiate(busPrefab, busSpawn.position, busSpawn.rotation);
      hasSpawn = false;
    }

  }
  bool setSpawBus()
  {
    if (currentTime > endTime)
    {
      currentTime -= Time.deltaTime;
      return false;
    }
    else
    {
      return true;
    }
  }
}
