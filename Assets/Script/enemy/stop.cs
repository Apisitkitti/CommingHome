using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stop : MonoBehaviour
{
  [SerializeField]
  Animator homelessPlay;
  void OnTriggerEnter(Collider playerCol)
  {
    if (playerCol.gameObject.tag == "enemy")
    {
      homelessPlay.SetTrigger("stand");
    }
  }
}
