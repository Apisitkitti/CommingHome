using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryStart : MonoBehaviour
{
  [SerializeField] TMP_Text storyText;
  [SerializeField] storyInside lines;
  [SerializeField] float textSpeed;
  [SerializeField] float uiDisappear;
  [SerializeField] DeathStorySet normalStory;
  [SerializeField] GameObject storyUi;
  int dialogueNumber;

  void Start()
  {
    storyText.text = string.Empty;
    storyUi.SetActive(false);
  }

  void storySetter()
  {

    foreach (bool storyIsCheck in normalStory.storyCheck)
    {
      if (storyIsCheck) dialogueNumber++;
      if (dialogueNumber >= lines.storyLine.Count) return;
    }
  }
  public void startDialogue()
  {
    storySetter();
    storyUi.SetActive(true);
    StartCoroutine(typeLine());

  }
  IEnumerator typeLine()
  {
    foreach (char charcterInConversation in lines.storyLine[dialogueNumber])
    {
      storyText.text += charcterInConversation;
      yield return new WaitForSeconds(textSpeed);
    }
    yield return new WaitForSeconds(uiDisappear);
    storyUi.SetActive(false);
  }
}
