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
  [SerializeField] float FadeSpeed;
  [Header("MessageAndImage")]
  // [SerializeField] List<string> deathMessage;
  [SerializeField] List<Sprite> deathImageBackground;
  [SerializeField] DeathStorySet deathCheck;

  [Header("Input Object")]
  // [SerializeField] TMP_Text deathText;
  [SerializeField] Image backGroundDeathimage;
  [SerializeField] Image coverImage;
  [SerializeField] CanvasGroup canvasGroup;
  [SerializeField] DeathStorySet deathStorySet;
  public int deathmessageNumber = 0;
  [HideInInspector] public int deathPictureNumber;
  private Color tempColor;

  void Awake()
  {
    tempColor = coverImage.color;
    tempColor.a = 1f;
    deathStorySetter(deathPictureNumber);
  }
  void Start()
  {
    StartCoroutine(deathMessageStart());
  }

  void deathStorySetter(int deathPictureNumber)
  {

    if (deathStorySet.storyCheck[deathPictureNumber])
    {
      deathmessageNumber = deathPictureNumber;
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
    deathStorySetter(deathPictureNumber);
    yield return new WaitForSeconds(wordAppear);
    setDeathImage(deathPictureNumber);
    StartCoroutine(FadePictureDeath());
    yield return new WaitForSeconds(FadeSpeed);
    StartCoroutine(FadeBackMenu());
    // resetDeathDialogueState(false);
    deathCheck.storyCheck[deathPictureNumber] = false;

  }

  IEnumerator FadePictureDeath()
  {
    while (tempColor.a > 0f)
    {
      tempColor.a -= Time.deltaTime * FadeSpeed;
      coverImage.color = tempColor;
      yield return null;
    }

  }

  IEnumerator FadeBackMenu()
  {
    while (canvasGroup.alpha < 1f)
    {
      canvasGroup.alpha += Time.deltaTime * FadeSpeed;
      yield return null;
    }

  }

  // public void resetDeathDialogueState(bool active)
  // {
  //   deathText.text = string.Empty;

  //   for (int storyboolNumber = 0; storyboolNumber < deathStorySet.storyCheck.Count; storyboolNumber++)
  //   {
  //     deathStorySet.storyCheck[storyboolNumber] = active;
  //   }
  // }
  public void setDeathImage(int deathPictureNumber)
  {
    backGroundDeathimage.sprite = deathImageBackground[deathPictureNumber];
  }
}


