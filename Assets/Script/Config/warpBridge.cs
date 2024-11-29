using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warpBridge : MonoBehaviour
{
  [SerializeField] List<Transform> onTheBridegePostion;
  [SerializeField] List<Transform> underTheBridgePosition;
  [SerializeField] List<Transform> playerObject;

  public void warpUp(int warpNumber)
  {
    foreach (Transform player in playerObject)
    {
      player.transform.position = onTheBridegePostion[warpNumber].position;
    }
  }
  public void warpDown(int warpNumber)
  {
    foreach (Transform player in playerObject)
    {
      player.transform.position = underTheBridgePosition[warpNumber].position;
    }
  }

}
