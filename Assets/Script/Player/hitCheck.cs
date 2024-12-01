using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hitCheck : MonoBehaviour
{
  [SerializeField] UiSetter uiSetter;
  [SerializeField] DeathStorySet deathStorySet;
  [SerializeField] SceneManage sceneChange;


  void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Car" || col.gameObject.tag == "Bus")
    {
      deathStorySet.storyCheck[0] = true;
      SceneManager.LoadScene("DeathScene");
    }
    if (col.gameObject.tag == "enemy")
    {
      deathStorySet.storyCheck[1] = true;
      SceneManager.LoadScene("DeathScene");
    }
    if (col.gameObject.tag == "enemyType2")
    {
      deathStorySet.storyCheck[2] = true;
      SceneManager.LoadScene("DeathScene");
    }

  }
  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "StoryHit")
    {
      sceneChange.nextScene();
    }
    if (col.gameObject.tag == "hole")
    {
      deathStorySet.storyCheck[3] = true;
      SceneManager.LoadScene("DeathScene");
    }
    if (col.gameObject.tag == "door")
    {
      SceneManager.LoadScene("EndScene");
    }
    if (col.gameObject.tag == "anotherStory")
    {
      sceneChange.Respawn();
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
