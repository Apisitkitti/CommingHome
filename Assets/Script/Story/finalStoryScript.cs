using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class finalStoryScript : MonoBehaviour
{
  [SerializeField] TMP_Text finalScriptText;
  [SerializeField] TMP_Text description;
  [SerializeField] TMP_Text main;
  [SerializeField] List<string> appearText;
  [SerializeField] GameObject MainMenuButton;
  [SerializeField] int timeToApear;
  void Start()
  {
    MainMenuButton.SetActive(false);
    StartCoroutine(textRun());
  }

  IEnumerator textRun()
  {

    mainText(appearText[0]);
    yield return new WaitForSeconds(timeToApear);
    mainText("");
    MainMenuButton.SetActive(true);
    setTextFinal(appearText[1]);
    setDescription("ขอขอบคุณที่เล่นเกมของเรากหวังว่าทุกคนจะชอบเกมของเรา");
  }
  void setTextFinal(string text)
  {
    finalScriptText.text = text;
  }
  void setDescription(string text)
  {
    description.text = text;
  }
  void mainText(string text)
  {
    main.text = text;
  }

}


