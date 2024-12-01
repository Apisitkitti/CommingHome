using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class answerQA : MonoBehaviour
{
  [SerializeField] List<string> moneyList;
  [SerializeField] TMP_Text moneyText;
  [SerializeField] Animator homeLessAnimator;
  [SerializeField] List<GameObject> answeUI;
  [SerializeField] DeathStorySet scarventureDo;
  [SerializeField] GameObject fieldToactiveAnim;


  int numberInList;
  void Start()
  {
    numberInList = 0;
  }
  public void moneyChanger1()
  {
    if (numberInList >= 2)
    {
      gameObject.SetActive(false);
    }
    else
    {
      moneyText.text = $"ขอเงินหน่อย{moneyList[numberInList]}";
      numberInList += 1;
    }
  }
  public void moneyChanger2()
  {
    if (numberInList >= 2)
    {
      gameObject.SetActive(false);
    }
    else if (numberInList >= 1 && numberInList <= 2)
    {
      moneyText.text = $"ขอเงินหน่อย{moneyList[numberInList]}";
      numberInList += 1;

    }
    else
    {
      moneyText.text = "บ้านอยู่ไหน";
      SwapAnswer(false);
      numberInList += 1;
    }
  }

  public void dontGiveMoney()
  {
    homeLessAnimator.SetBool("yellActive", true);
  }
  public void triggerLastScene()
  {
    scarventureDo.storyCheck[0] = true;
    homeLessAnimator.SetBool("yellActive", false);
    fieldToactiveAnim.SetActive(false);
  }
  public void dontTellHim()
  {
    homeLessAnimator.SetBool("yellActive", true);
  }
  public void SwapAnswer(bool isAppear)
  {
    answeUI[0].SetActive(isAppear);
    answeUI[1].SetActive(!isAppear);
  }

}
