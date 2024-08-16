using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSetter : MonoBehaviour
{
  [SerializeField] GameObject busUIForWait;
  [SerializeField] GameObject busUIForDontWait;
  [SerializeField] GameObject takeABusUI;
  [SerializeField] GameObject wrapBusButton;

  public void setBusUI(bool isActive)
  {
    busUIForWait.SetActive(isActive);
    busUIForDontWait.SetActive(!isActive);
  }
  public void setTakeABusUI(bool isActive)
  {
    takeABusUI.SetActive(isActive);
    busUIForDontWait.SetActive(!isActive);
  }
  public void setOverAllBusUi(bool isActive)
  {

    wrapBusButton.SetActive(isActive);
  }
}
