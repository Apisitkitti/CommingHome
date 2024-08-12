using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathMessageChecker", menuName = "DeathMessage")]
public class DeathStorySet : ScriptableObject
{
  public List<bool> storyCheck;
}
