using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopBusHit : MonoBehaviour
{
  [SerializeField] UiSetter setUiActive;
  [SerializeField] StoryStart setEvent;
  [SerializeField] storyInside storyData;

  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "Bus")
      setUiActive.setTakeABusUI(true);
    setEvent.setText(storyData.eventString[2]);
  }
}
