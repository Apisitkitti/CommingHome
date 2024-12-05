using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class answerQA : MonoBehaviour
{
  [SerializeField] List<string> moneyList;
  [SerializeField] TMP_Text moneyText;
  [SerializeField] Animator homeLessAnimator;
  [SerializeField] List<GameObject> answeUI;
  [SerializeField] GameObject fieldToactiveAnim;
  [SerializeField] OVRPlayerController walkspeed;
  [SerializeField] GameObject platform;
  [SerializeField] GameObject door;

  [SerializeField] int numberInList;
  void Start()
  {
    moneyText.text = $"ขอเงินหน่อย {moneyList[numberInList]} บาท";
    door.SetActive(false);
  }
  public void moneyChanger1()
  {
    if (numberInList < moneyList.Count && numberInList > moneyList.Count - 2)
    {
      walkspeed.Acceleration = 0.08f;
      Destroy(platform);
      Destroy(gameObject);
    }
    else
    {
      numberInList += 1;
      moneyText.text = $"ขอเงินหน่อย {moneyList[numberInList]} บาท";

    }
  }
  public void moneyChanger2()
  {
    if (numberInList >= moneyList.Count)
    {
      walkspeed.Acceleration = 0.08f;
      Destroy(platform);
      Destroy(gameObject);
    }
    else if (numberInList >= 0 && numberInList < moneyList.Count - 1)
    {
      numberInList += 1;
      moneyText.text = $"ขอเงินหน่อย {moneyList[numberInList]} บาท";
    }
    else
    {
      moneyText.text = "บ้านอยู่ไหน";
      SwapAnswer(false);
    }
  }

  public void dontGiveMoney()
  {
    walkspeed.Acceleration = 0.08f;
    homeLessAnimator.SetBool("yellActive", true);
    Destroy(platform);
    Destroy(gameObject);
  }
  public void triggerLastScene()
  {
    walkspeed.Acceleration = 0.08f;
    homeLessAnimator.SetBool("yellActive", false);
    fieldToactiveAnim.SetActive(false);
    door.SetActive(true);
    Destroy(platform);
    Destroy(gameObject);
  }
  public void dontTellHim()
  {
    walkspeed.Acceleration = 0.08f;
    homeLessAnimator.SetBool("stand", false);
    Destroy(platform);
    Destroy(gameObject);
  }
  public void SwapAnswer(bool isAppear)
  {
    for (int i = 0; i < answeUI.Count; i++)
    {
      if (i < 2)
      {
        answeUI[i].SetActive(isAppear);
      }
      else if (i >= 2)
      {
        answeUI[i].SetActive(!isAppear);
      }
    }
  }

}
