using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class notification : MonoBehaviour
{
  [SerializeField] List<Sprite> notiImage;
  [SerializeField] Image notiShow;
  [SerializeField] GameObject UiObject;




  void Start()
  {
    UiObject.SetActive(false);
  }

  public void notiChanger(int notiNumber)
  {
    notiShow.sprite = notiImage[notiNumber];
    UiObject.SetActive(true);
  }
}
