using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerAnimation : MonoBehaviour
{
  [SerializeField] Animator homeLessAnimator;

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
}
