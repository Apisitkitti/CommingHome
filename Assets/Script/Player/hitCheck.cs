using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hitCheck : MonoBehaviour
{
  [SerializeField] UiSetter uiSetter;
  [SerializeField] DeathStorySet normalStorySet;
  [SerializeField] Transform warpZone;

  void Start()
  {
    if (warpZone == null)
    {
      return;
    }
  }
  void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Car" || col.gameObject.tag == "Bus")
    {
      for (int storySet = 0; storySet < normalStorySet.storyCheck.Count; storySet++)
      {
        normalStorySet.storyCheck[storySet] = false;
      }
      SceneManager.LoadScene("DeathScene");
    }
  }
  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "StoryHit")
    {
      SceneManager.LoadScene("BusScene");
    }
  }
  void OnTriggerStay(Collider col)
  {
    if (col.gameObject.tag == "HitBox")
    {
      dontWantErrorFunction();
      uiSetter.setOverAllBusUi(true);
    }
    if (col.gameObject.tag == "bridgeWarp")
    {

    }
  }
  void OnTriggerExit(Collider col)
  {
    if (col.gameObject.tag == "HitBox")
    {
      dontWantErrorFunction();
      uiSetter.setOverAllBusUi(false);
    }
  }

  void dontWantErrorFunction()
  {
    if (uiSetter == null) return;
    if (warpZone == null) return;
  }
}
