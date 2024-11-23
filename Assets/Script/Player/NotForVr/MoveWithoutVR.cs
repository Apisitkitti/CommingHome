using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 5f;
  public float cameraSensitivity = 100f;
  public AudioSource walkingSound;

  private Vector3 move;
  private float rotationX = 0f;
  private float rotationY = 0f;

  // Assign the camera in the Inspector
  public Camera playerCamera;

  void Start()
  {
    if (playerCamera == null)
    {
      playerCamera = Camera.main; // Use the main camera if none is assigned
    }
  }

  void FixedUpdate()
  {
    Movement();
    CameraMoveWithMouse();
  }

  void Movement()
  {
    float moveX = Input.GetAxisRaw("Horizontal");
    float moveY = Input.GetAxisRaw("Vertical");

    // Get the camera's forward and right directions, projected onto the horizontal plane
    Vector3 forward = playerCamera.transform.forward;
    Vector3 right = playerCamera.transform.right;

    forward.y = 0f; // Remove vertical component
    right.y = 0f;   // Remove vertical component

    forward.Normalize();
    right.Normalize();

    // Combine movement directions
    move = (right * moveX + forward * moveY).normalized * moveSpeed * Time.deltaTime;

    // Move the player
    transform.position += move;

    // Play walking sound
    if (move != Vector3.zero)
    {
      if (!walkingSound.isPlaying)
      {
        walkingSound.Play();
      }
    }
  }

  void CameraMoveWithMouse()
  {
    float cameraMoveX = Input.GetAxisRaw("Mouse Y");
    float cameraMoveY = Input.GetAxisRaw("Mouse X");

    rotationX += cameraMoveX * -1 * cameraSensitivity * Time.deltaTime;
    rotationY += cameraMoveY * cameraSensitivity * Time.deltaTime;

    // Clamp vertical rotation to prevent flipping
    rotationX = Mathf.Clamp(rotationX, -90f, 90f);

    transform.localEulerAngles = new Vector3(0f, rotationY, 0f);
    playerCamera.transform.localEulerAngles = new Vector3(rotationX, 0f, 0f);
  }
}
