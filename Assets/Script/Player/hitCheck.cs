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
      normalStorySet.storyCheck[0] = true;
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

  }
}
