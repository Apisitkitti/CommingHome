using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPoint : MonoBehaviour
{
  [SerializeField] StoryStart story;

  void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Player")
    {
      story.startDialogue();
    }
  }

}
