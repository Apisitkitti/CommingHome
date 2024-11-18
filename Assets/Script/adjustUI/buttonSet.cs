using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSet : MonoBehaviour
{
  [SerializeField] BusSpawn busSpawn;
  [SerializeField] UiSetter busUi;
  [SerializeField] OVRPlayerController VrMove;
  float indexSpeed;
  public bool Clickwaitbus = false;
  void Start()
  {
    indexSpeed = VrMove.Acceleration;
  }
  public void setBusSpawn()
  {
    setActiveBus(true);
    VrMove.Acceleration = 0;
    Clickwaitbus = true;
  }
  public void dontWaitBus()
  {
    setActiveBus(false);
    VrMove.Acceleration = indexSpeed;
    Clickwaitbus = false;
  }
  void setActiveBus(bool isActive)
  {
    busSpawn.enabled = isActive;
    busUi.setBusUI(!isActive);
  }
}