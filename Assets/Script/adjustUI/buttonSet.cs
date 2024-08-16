using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSet : MonoBehaviour
{
  [SerializeField] BusSpawn busSpawn;
  [SerializeField] UiSetter busUi;
  public void setBusSpawn()
  {
    busSpawn.enabled = true;
    busUi.setBusUI(false);
  }
  public void dontWaitBus()
  {
    busSpawn.enabled = false;
    busUi.setBusUI(true);
  }
}