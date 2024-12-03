using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class notification : MonoBehaviour
{
  [SerializeField] storyInside notiImage;
  [SerializeField] Image notiShow;
  [SerializeField] GameObject UiObject;
  [SerializeField] List<notficaitonplace> notiDelay;
  [SerializeField] int timeForShowUi;



  void Start()
  {
    UiObject.SetActive(false);
  }

  public void notiChanger(int notiNumber)
  {
    notiShow.sprite = notiImage.notificationImage[notiNumber];
    UiObject.SetActive(true);
    for (int start = 0; start < notiDelay.Count; start++) { notiDelay[start].notiNumberForShow = timeForShowUi; }
  }
}
