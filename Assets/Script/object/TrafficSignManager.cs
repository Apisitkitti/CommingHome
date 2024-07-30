using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSignManager : MonoBehaviour
{
  [SerializeField] List<MeshRenderer> lightBulb;
  [SerializeField] List<Material> color;
  public int changeLight;
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
      yield return new WaitForSeconds(changeLight);
      lightBulb[0].material = color[0];
      lightBulb[1].material = color[2];
      yield return new WaitForSeconds(changeLight);
      lightBulb[1].material = color[0];
      lightBulb[2].material = color[3];
      yield return new WaitForSeconds(changeLight);
    }
  }


}
