using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSetter : MonoBehaviour
{
  [SerializeField] GameObject busUIForWait;
  [SerializeField] GameObject busUIForDontWait;

  public void setBusUI(bool isActive)
  {
    busUIForWait.SetActive(isActive);
    busUIForDontWait.SetActive(!isActive);
  }
}
