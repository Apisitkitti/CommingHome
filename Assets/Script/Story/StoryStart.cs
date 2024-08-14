using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryStart : MonoBehaviour
{
  [SerializeField] TMP_Text storyText;
  [SerializeField] GameObject checkPoint;
  [SerializeField] string lines;
  [SerializeField] float textSpeed;

  void Start()
  {
    storyText.text = string.Empty;
  }
  public void startDialogue()
  {
    StartCoroutine(typeLine());
    Destroy(checkPoint);

  }
  IEnumerator typeLine()
  {
    foreach (char charcterInConversation in lines)
    {
      storyText.text += charcterInConversation;
      yield return new WaitForSeconds(textSpeed);
    }
  }
}
