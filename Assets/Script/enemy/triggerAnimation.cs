using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class triggerAnimation : MonoBehaviour
{
  [SerializeField] Animator homeLessAnimator;
  [SerializeField] OVRPlayerController canWalkNow;
  [SerializeField] GameObject answerQuestionUi;

  void Start()
  {
    answerQuestionUi.SetActive(false);
  }
  void OnTriggerStay(Collider col)
  {
    if (col.gameObject.tag == "Player")
    {
      homeLessAnimator.SetBool("yellActive", true);
    }
  }
  void OnTriggerExit(Collider col)
  {
    if (col.gameObject.tag == "Player")
    {
      homeLessAnimator.SetBool("yellActive", false);
    }
  }
  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "Player")
    {
      canWalkNow.Acceleration = 0;
      answerQuestionUi.SetActive(true);
    }
  }
}

