using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSignManager : MonoBehaviour
{
  [SerializeField] List<MeshRenderer> lightBulb;
  [SerializeField] List<Material> color;
  [SerializeField] CarSpawn carSpawn;
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
      carSpawn.isSpawn.Clear();
      lightBulb[0].material = color[1];
      lightBulb[2].material = color[0];
      carSpawn.isSpawn.Add(false);
      yield return new WaitForSeconds(changeLightRedToYellow);
      lightBulb[0].material = color[0];
      lightBulb[1].material = color[2];
      yield return new WaitForSeconds(changeLightYellowToGreen);
      lightBulb[1].material = color[0];
      lightBulb[2].material = color[3];
      carSpawn.isSpawn.Add(true);
      yield return new WaitForSeconds(changeLightGreenToRed);
    }
  }


}
