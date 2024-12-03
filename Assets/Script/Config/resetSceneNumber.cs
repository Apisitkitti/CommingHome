using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetSceneNumber : MonoBehaviour
{
  [SerializeField] checkSceneToRespawn indexScene;
  void Start()
  {
    indexScene.sceneNumber = 1;
  }
}
