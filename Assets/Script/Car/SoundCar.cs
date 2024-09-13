using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCar : MonoBehaviour
{
    [SerializeField] AudioSource audiosource;
    [SerializeField] Transform Player;
    [SerializeField] float DistReduceAt = 5f;

    void Start(){
        GameObject playerTag = GameObject.FindWithTag("Player");
        Player =playerTag.transform;
    }
    void Update()
    {
        float dist = Vector3.Distance(transform.position, Player.position);
        if(dist <= audiosource.maxDistance)
        {
            float ReduceSound = Mathf.Clamp01(1 - (dist - DistReduceAt) / (audiosource.maxDistance - DistReduceAt));
            audiosource.volume = ReduceSound;
            if(!audiosource.isPlaying){
                audiosource.Play();
            }
        }
        else
        {
            if(audiosource.isPlaying){
                audiosource.Stop();
            }
        }

    }
}
