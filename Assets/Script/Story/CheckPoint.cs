using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPoint : MonoBehaviour
{
  [SerializeField] TMP_Text storyText;
  [SerializeField] string lines;
  [SerializeField] float textSpeed;
  [SerializeField] bool isHit = false;
  private int index;
  void Start()
  {
    storyText.text = string.Empty;
  }
  void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Player")
    {
      startDialogue();
      isHit = true;
    }
  }
  void startDialogue()
  {
    index = 0;
    StartCoroutine(typeLine());

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
