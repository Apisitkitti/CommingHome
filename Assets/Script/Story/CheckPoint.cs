using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPoint : MonoBehaviour
{
  [SerializeField] StoryStart story;
  [SerializeField] DeathStorySet storySetter;
  int storyNumber = 0;
  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "Player")
    {
      story.startDialogue();
      storySetter.storyCheck[storyNumber] = true;
      storyNumber++;
      Destroy(gameObject);
    }
  }

}
