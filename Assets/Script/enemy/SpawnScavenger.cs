using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SpawnScavenger : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;

    void OnTriggerEnter(Collider other)
    {   
        if (other.CompareTag("Player")){
            onTriggerEnter.Invoke();
        }
        
    }

    /*private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke();
    }*/
}
