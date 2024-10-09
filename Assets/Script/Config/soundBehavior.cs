using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundBehavior : MonoBehaviour
{
  [SerializeField] AudioSource walkingSound;

  public void playWalkSound()
  {
    walkingSound.Play();
  }
}
