using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithoutVR : MonoBehaviour
{
  #region movementControl
  public float moveSpeed = 1f;
  #endregion
  #region CameraSetting
  float rotationX = 0f;
  float rotationY = 0f;
  [SerializeField] float cameraSentivity = 15f;
  [SerializeField] AudioSource walkingSound;
  #endregion

  Vector3 move;

  void Start()
  {

  }
  void FixedUpdate()
  {
    movement();
    cameraMoveWithMouse();
  }

  void movement()
  {
    float moveX = Input.GetAxisRaw("Horizontal");
    float moveY = Input.GetAxisRaw("Vertical");
    move = new Vector3(moveX, 0f, moveY);
    move = move * moveSpeed * Time.deltaTime;
    transform.position += move;
     if (move != Vector3.zero)
        {
            if (!walkingSound.isPlaying)
            {
                walkingSound.Play();
            }
        }
  }

  void cameraMoveWithMouse()
  {
    float cameraMoveX = Input.GetAxisRaw("Mouse Y");
    float cameraMoveY = Input.GetAxisRaw("Mouse X");
    rotationX += cameraMoveX * -1 * cameraSentivity * Time.deltaTime;
    rotationY += cameraMoveY * cameraSentivity * Time.deltaTime;
    transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);


  }
}
