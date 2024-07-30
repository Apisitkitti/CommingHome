using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitCheck : MonoBehaviour
{
  [SerializeField] respawnScript respawn;
  void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Car")
    {
      respawn.respawnPlayer();
    }
  }
}
