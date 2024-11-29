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
    sceneNumber.sceneNumber += 1;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }
  public void previousScene()
  {
    sceneNumber.sceneNumber -= 1;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
  }
}
