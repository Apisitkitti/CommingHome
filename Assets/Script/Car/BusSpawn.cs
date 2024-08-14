using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusSpawn : MonoBehaviour
{
  [SerializeField] GameObject busPrefab;
  [SerializeField] Transform busSpawn;
  [SerializeField] int timeSpawn;

  void Start()
  {
    StartCoroutine(busSpawner());
  }
  IEnumerator busSpawner()
  {
    yield return new WaitForSeconds(timeSpawn);
    var busSpawnCar = Instantiate(busPrefab, busSpawn.position, busSpawn.rotation);
  }
}
