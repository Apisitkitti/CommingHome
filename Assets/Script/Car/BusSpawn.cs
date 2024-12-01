using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BusSpawn : MonoBehaviour
{
  [SerializeField] GameObject busPrefab;
  [SerializeField] Transform busSpawn;
  [SerializeField] float timeSpawn;
  [SerializeField] storyInside eventStory;
  [SerializeField] StoryStart storyStart;
  [SerializeField] float playTextNumber;
  public float currentTime;
  private float endTime = 0;
  bool hasSpawn = true;
  void Start()
  {
    currentTime = timeSpawn;
    storyStart.setText("");
    playTextNumber = timeSpawn - 10;
  }
  void Update()
  {
    setSpawBus();
    busSpawner();

  }
  void busSpawner()
  {
    if (setSpawBus() && hasSpawn)
    {
      var busSpawnCar = Instantiate(busPrefab, busSpawn.position, busSpawn.rotation);
      hasSpawn = false;
    }
    if (currentTime <= playTextNumber && currentTime >= 48)
    {
      storyStart.setText(eventStory.eventString[0]);
    }

  }
  bool setSpawBus()
  {
    if (currentTime > endTime)
    {
      currentTime -= Time.deltaTime;
      return false;
    }
    else
    {
      return true;
    }
  }
}
