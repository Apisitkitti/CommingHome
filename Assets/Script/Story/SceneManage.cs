using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManage : MonoBehaviour
{
  public void Respawn()
  {
    SceneManager.LoadScene("Scene2");
  }
  public void nextScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }
  public void previousScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
  }
}
