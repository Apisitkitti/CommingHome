using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSet : MonoBehaviour
{
  [SerializeField] BusSpawn busSpawn;
  [SerializeField] GameObject destroyUI;
  public void setBusSpawn()
  {
    busSpawn.enabled = true;
    Destroy(destroyUI);

  }
  public void dontWaitBus()
  {
    busSpawn.enabled = false;
  }
}