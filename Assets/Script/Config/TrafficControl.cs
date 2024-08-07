using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrafficBoolean", menuName = "trafficControl")]
public class TrafficControl : ScriptableObject
{
  public List<bool> carSpawnEnable;
}
