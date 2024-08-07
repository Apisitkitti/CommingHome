using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSignManager : MonoBehaviour
{
  public TrafficControl trafficControl;
  [SerializeField] List<MeshRenderer> lightBulb;
  [SerializeField] List<Material> color;
  [SerializeField] CarSpawn carSpawn;

  [Header("spawnNumber Start With number one")]
  [SerializeField] int trafficNumber;

  [Header("color")]
  public int changeLightRedToYellow;
  public int changeLightYellowToGreen;
  public int changeLightGreenToRed;

  void Start()
  {
    lightBulb[0].material = color[1];
    StartCoroutine(lightChanger());
  }

  IEnumerator lightChanger()
  {
    while (true)
    {

      lightBulb[0].material = color[1];
      lightBulb[2].material = color[0];
      trafficControl.carSpawnEnable[trafficNumber - 1] = false;
      yield return new WaitForSeconds(changeLightRedToYellow);

      lightBulb[0].material = color[0];
      lightBulb[1].material = color[2];
      yield return new WaitForSeconds(changeLightYellowToGreen);

      lightBulb[1].material = color[0];
      lightBulb[2].material = color[3];
      trafficControl.carSpawnEnable[trafficNumber - 1] = true;
      yield return new WaitForSeconds(changeLightGreenToRed);
    }
  }
}

