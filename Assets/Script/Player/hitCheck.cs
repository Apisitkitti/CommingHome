using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hitCheck : MonoBehaviour
{
  [SerializeField] UiSetter uiSetter;
  void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Car")
    {
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
      uiSetter.setOverAllBusUi(true);
    }
  }
  void OnTriggerExit(Collider col)
  {
    if (col.gameObject.tag == "HitBox")
    {
      uiSetter.setOverAllBusUi(false);
    }
  }
}
