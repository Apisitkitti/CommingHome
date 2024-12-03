using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "normalStoryLine")]
public class storyInside : ScriptableObject
{
  public List<string> storyLine;
  public List<string> eventString;
  public List<Sprite> notificationImage;
}
