using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPoint : MonoBehaviour
{
  [SerializeField] StoryStart story;

  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "Player")
    {
      story.startDialogue();
      Destroy(gameObject);
    }
  }

}
