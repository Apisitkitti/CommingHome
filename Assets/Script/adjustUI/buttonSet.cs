using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSet : MonoBehaviour
{
  [SerializeField] BusSpawn busSpawn;
  public void setBusSpawn()
  {
    busSpawn.enabled = true;
  }
  public void dontWaitBus()
  {
    busSpawn.enabled = false;
  }
}