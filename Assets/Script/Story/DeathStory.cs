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
  public int deathmessageNumber = 0;


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
    foreach (bool deathTextSceneChecker in deathStorySet.storyCheck)
    {
      if (deathStorySet.storyCheck[deathmessageNumber])
      {
        deathmessageNumber++;
      }
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
    resetDeathDialogueState(false);
  }
  void resetDeathDialogueState(bool active)
  {
    deathText.text = string.Empty;

    for (int storyboolNumber = 0; storyboolNumber < deathStorySet.storyCheck.Count; storyboolNumber++)
    {
      deathStorySet.storyCheck[storyboolNumber] = active;
    }
  }
}


