using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stand : MonoBehaviour
{
  [SerializeField] Animator standTrack;

  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "Player")
    {
      standTrack.SetTrigger("stand");
    }
  }
}
