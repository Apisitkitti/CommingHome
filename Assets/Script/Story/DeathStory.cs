using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathStory : MonoBehaviour
{
  [Header("TimeForWordSetting")]
  [SerializeField] int timeBetweenWord;
  [SerializeField] int wordAppear;
  [Header("Message")]
  [SerializeField] List<string> deathMessage;

  [Header("Input Object")]
  [SerializeField] TMP_Text deathText;
  [SerializeField] DeathStorySet deathStorySet;
  int deathmessageNumber = 0;


  void Awake()
  {
    deathStorySetter();
  }
  void Start()
  {
    StartCoroutine(deathMessageStart());
  }

  void deathStorySetter()
  {
    if (deathStorySet.storyCheck[deathmessageNumber])
    {
      deathmessageNumber++;
    }
  }
  IEnumerator deathMessageStart()
  {
    foreach (char characterInWords in deathMessage[deathmessageNumber])
    {
      deathText.text += characterInWords;
      yield return new WaitForSeconds(timeBetweenWord);
    }
    yield return new WaitForSeconds(wordAppear);
    deathText.text = "";

  }
}
