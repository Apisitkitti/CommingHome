using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class animationStart : MonoBehaviour
{
  [SerializeField] Animator homelessPlay;
  [SerializeField] SplineAnimate walkNow;

  [SerializeField] CapsuleCollider cap;
  void Start()
  {
    walkNow.enabled = false;
    cap.enabled = false;
  }
  void OnTriggerEnter(Collider playerCol)
  {
    if (playerCol.gameObject.tag == "Player")
    {
      walkNow.enabled = true;
      cap.enabled = true;
      homelessPlay.SetTrigger("walk");

    }
    if (playerCol.gameObject.tag == "enemy")
    {
      homelessPlay.SetTrigger("stand");
    }
  }
}
