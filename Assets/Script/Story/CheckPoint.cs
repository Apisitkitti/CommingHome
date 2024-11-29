using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPoint : MonoBehaviour
{
  [SerializeField] StoryStart story;
  [SerializeField] DeathStorySet storySetter;
  public int storyCheckNumber;
  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "Player")
    {
      story.storyNumber = storyCheckNumber;
      story.startDialogue();
      storySetter.storyCheck[story.storyNumber] = true;
      // Destroy(gameObject);
    }
  }

}
