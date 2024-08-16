using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopBusHit : MonoBehaviour
{
  [SerializeField] UiSetter setUiActive;

  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "Bus")
      setUiActive.setTakeABusUI(true);
  }
}
