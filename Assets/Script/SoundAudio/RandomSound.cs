using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource audioSource;
    public buttonSet buttonSet;
    private int currentClipIndex = -1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
            RandomSoundWalk();
        }
    }

    void RandomSoundWalk()
    {
        if (sounds.Length > 0)
        {
            // Randomize the next clip, ensuring it's not the same as the previous one
            int nextClipIndex;
            do
            {
                nextClipIndex = Random.Range(0, sounds.Length);
            } while (nextClipIndex == currentClipIndex);

            currentClipIndex = nextClipIndex; // Update the current clip index
            audioSource.clip = sounds[currentClipIndex]; 
            audioSource.Play(); 
        }
    }
}
