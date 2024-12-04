using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notficaitonplace : MonoBehaviour
{
  [SerializeField] int storySendNumber;
  [SerializeField] notification setNoti;
  [SerializeField] GameObject uiNoti;
  [SerializeField] AudioSource audioSource;
  [SerializeField] private bool PlaySoundOneTime;
  public int notiNumberForShow;

  void OnTriggerEnter(Collider playerTag)
  {
    if (playerTag.gameObject.tag == "Player")
    {
      StartCoroutine(delayNoti());
      StartSoundNotiLine();
    }
  }
  IEnumerator delayNoti()
  {
    setNoti.notiChanger(storySendNumber);
    yield return new WaitForSeconds(notiNumberForShow);
    uiNoti.SetActive(false);
    Destroy(gameObject);
  }

  private bool StartSoundNotiLine()
  {
    if(PlaySoundOneTime == false)
    {
      audioSource.Play();
      PlaySoundOneTime = true;
    }
    return PlaySoundOneTime;
  }
}
