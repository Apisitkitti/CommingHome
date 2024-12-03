using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notficaitonplace : MonoBehaviour
{
  [SerializeField] int storySendNumber;
  [SerializeField] notification setNoti;
  [SerializeField] GameObject uiNoti;
  public int notiNumberForShow;

  void OnTriggerEnter(Collider playerTag)
  {
    if (playerTag.gameObject.tag == "Player")
    {
      StartCoroutine(delayNoti());
    }
  }
  IEnumerator delayNoti()
  {
    setNoti.notiChanger(storySendNumber);
    yield return new WaitForSeconds(notiNumberForShow);
    uiNoti.SetActive(false);
    Destroy(gameObject);
  }
}
