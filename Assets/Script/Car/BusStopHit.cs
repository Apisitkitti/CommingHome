using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopHit : MonoBehaviour
{
  [SerializeField] CarMove busMove;
  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "stop")
    {
      busMove.carSpeed = 0;
    }
  }
}
