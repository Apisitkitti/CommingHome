using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetSceneNumber : MonoBehaviour
{
  [SerializeField] checkSceneToRespawn indexScene;
  [SerializeField] int sceneNumber;
  void Start()
  {
    indexScene.sceneNumber = sceneNumber;
  }
}
