using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bridgeUISet : MonoBehaviour
{
  [SerializeField] GameObject bridgeInterface;


  void Start()
  {
    bridgeInterface.SetActive(false);
  }

  void OnTriggerStay(Collider col)
  {
    if (col.gameObject.tag == "Player")
    {
      bridgeInterface.SetActive(true);
    }
  }
  void OnTriggerExit(Collider col)
  {
    if (col.gameObject.tag == "Player")
    {
      bridgeInterface.SetActive(false);
    }
  }
}