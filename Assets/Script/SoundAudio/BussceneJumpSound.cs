using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BussceneJumpSound : MonoBehaviour
{
  [SerializeField] BusSpawn busSpawn;
  [SerializeField] buttonSet buttonSet;
  private AudioSource audioSource;
  [SerializeField] float TimeJumpScaredSound;
  [SerializeField] public AudioClip[] sounds;
  [SerializeField] private bool JumpScaredSoundPlay = false;
  [SerializeField] StoryStart storyUi;
  [SerializeField] storyInside eventStory;
  // Start is called before the first frame update
  void Start()
  {
    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
    if (buttonSet.Clickwaitbus == false)
    {
      if (audioSource.isPlaying)
      {
        audioSource.Stop();
      }

    }
    else if (buttonSet.Clickwaitbus == true && !audioSource.isPlaying)
    {
      PlayEmbientSound();
    }
    else if (busSpawn.currentTime <= TimeJumpScaredSound && JumpScaredSoundPlay == false)
    {
      JumpScaredSound();
      storyUi.setText(eventStory.eventString[1]);
      JumpScaredSoundPlay = true;
    }
  }

  void PlayEmbientSound()
  {
    audioSource.clip = sounds[0];
    audioSource.Play();
  }

  void JumpScaredSound()
  {
    audioSource.clip = sounds[1];
    audioSource.Play();

  }
}
