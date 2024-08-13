using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeathStory : MonoBehaviour
{
  [Header("TimeForWordSetting")]
  [SerializeField] int timeBetweenWord;
  [SerializeField] int wordAppear;
  [Header("MessageAndImage")]
  [SerializeField] List<string> deathMessage;
  [SerializeField] List<Sprite> deathImageBackground;

  [Header("Input Object")]
  [SerializeField] TMP_Text deathText;
  [SerializeField] Image backGroundDeathimage;
  [SerializeField] Image coverImage;

  [SerializeField] DeathStorySet deathStorySet;
  public int deathmessageNumber = 0;
  private Color tempColor;

  void Awake()
  {
    tempColor = coverImage.color;
    tempColor.a = 0;
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
    coverImage.color = Color.black;
    yield return new WaitForSeconds(timeBetweenWord);
    // foreach (char characterInWords in deathMessage[deathmessageNumber])
    // {
    //   deathText.text += characterInWords;
    //   yield return new WaitForSeconds(timeBetweenWord);
    // }
    deathText.text = deathMessage[deathmessageNumber];
    yield return new WaitForSeconds(timeBetweenWord);//สร้างตัวแปรใหม่เอานะ
    coverImage.color = tempColor;
    backGroundDeathimage.sprite = deathImageBackground[deathmessageNumber];
    yield return new WaitForSeconds(wordAppear);

    resetDeathDialogueState(false);
  }
  public void resetDeathDialogueState(bool active)
  {
    deathText.text = string.Empty;

    for (int storyboolNumber = 0; storyboolNumber < deathStorySet.storyCheck.Count; storyboolNumber++)
    {
      deathStorySet.storyCheck[storyboolNumber] = active;
    }
  }
}


