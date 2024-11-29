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
  public int storyNumber;
  int dialogueNumber;

  void Start()
  {
    storyText.text = string.Empty;
    storyUi.SetActive(false);
  }

  void storySetter(int storyNumber)
  {
    if (normalStory.storyCheck[storyNumber])
    {
      dialogueNumber = storyNumber;
    }
  }
  public void startDialogue()
  {
    storySetter(storyNumber);
    StartCoroutine(typeLine());

  }
  IEnumerator typeLine()
  {
    string story = lines.storyLine[dialogueNumber];
    setText(story);
    yield return new WaitForSeconds(textSpeed);
    yield return new WaitForSeconds(uiDisappear);
    storyUi.SetActive(false);
    normalStory.storyCheck[dialogueNumber] = false;
  }
  public void setText(string story)
  {
    storyUi.SetActive(true);
    storyText.text = story;
  }
}
