using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSet : MonoBehaviour
{
  [SerializeField] BusSpawn busSpawn;
  [SerializeField] UiSetter busUi;
  [SerializeField] OVRPlayerController VrMove;
  float indexSpeed;
  void Start()
  {
    indexSpeed = VrMove.Acceleration;
  }
  public void setBusSpawn()
  {
    setActiveBus(true);
    VrMove.Acceleration = 0;
  }
  public void dontWaitBus()
  {
    setActiveBus(false);
    VrMove.Acceleration = indexSpeed;
  }
  void setActiveBus(bool isActive)
  {
    busSpawn.enabled = isActive;
    busUi.setBusUI(!isActive);
  }
}