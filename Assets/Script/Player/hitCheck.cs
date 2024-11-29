using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hitCheck : MonoBehaviour
{
  [SerializeField] UiSetter uiSetter;
  [SerializeField] DeathStorySet normalStorySet;
  [SerializeField] SceneManage sceneChange;

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
      sceneChange.nextScene();
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
      uiSetter.OnTheBridegeUi(true);
    }
    if (col.gameObject.tag == "ontheBridgeWarp")
    {
      uiSetter.OnTheBridegeUi(true);
    }
  }
  void OnTriggerExit(Collider col)
  {
    if (col.gameObject.tag == "HitBox")
    {
      dontWantErrorFunction();
      uiSetter.setOverAllBusUi(false);
    }
    if (col.gameObject.tag == "bridgeWarp")
    {
      uiSetter.underTheBridegeUi(false);
    }
    if (col.gameObject.tag == "onTheBridgeWarp")
    {
      uiSetter.OnTheBridegeUi(false);
    }
  }

  void dontWantErrorFunction()
  {
    if (uiSetter == null) return;

  }
}
