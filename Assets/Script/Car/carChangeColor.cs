using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carChangeColor : MonoBehaviour
{
  [SerializeField] List<Material> carColor;
  [SerializeField] MeshRenderer thisCarColor;
  int randomMat;

  void Start()
  {
    randomMat = Random.Range(0, carColor.Count);
    thisCarColor.material = carColor[randomMat];
  }
}
