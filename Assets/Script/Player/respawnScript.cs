using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnScript : MonoBehaviour
{
  [SerializeField] List<Transform> playerSpawnPoint;
  [SerializeField] List<Transform> PlayerObject;
  public int spawnPointSet = 0;
  public void respawnPlayer()
  {
    foreach (Transform playerObject in PlayerObject)
    {
      playerObject.position = playerSpawnPoint[spawnPointSet].position;
    }
  }

}
