using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManage : MonoBehaviour
{
  [SerializeField] checkSceneToRespawn sceneNumber;
  public void Respawn()
  {
    SceneManager.LoadScene(sceneNumber.sceneNumber);
  }
  public void nextScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    sceneNumber.sceneNumber += 1;
  }
  public void previousScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    sceneNumber.sceneNumber -= 1;
  }
}
