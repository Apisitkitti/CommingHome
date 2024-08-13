using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hitCheck : MonoBehaviour
{
  [SerializeField] respawnScript respawn;
  [SerializeField] DeathStorySet articleDeathChecker;

  void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Car")
    {
      // respawn.respawnPlayer();
      SceneManager.LoadScene("DeathScene");
    }
  }
}
