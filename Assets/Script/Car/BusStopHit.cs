using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopHit : MonoBehaviour
{
  [SerializeField] CarMove busMove;
  [SerializeField] float timeToStopBus;
  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "stop")
    {
      StartCoroutine(delayStopBus());
    }
  }
  IEnumerator delayStopBus()
  {
    yield return new WaitForSeconds(timeToStopBus);
    busMove.carSpeed = 0;
  }
}
