using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
  public float carSpeed = 10f;      // Speed of the car
  public float turnSpeed = 100f;
  public float downForce = 10f;   // Speed of rotation
  private Rigidbody rb;
  private Vector3 initForward;
  private Quaternion targetRotation;
  private bool isTurningLeft = false;
  private bool isTurningRight = false;
  private float rotationAngle = 90f;  // Angle to turn

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    initForward = transform.forward;
  }

  void Update()
  {
    carForward(); // Move the car forward continuously
    handleCarTurn();
  }
  void FixedUpdate()
  {
    ApplyDownForce();
  }

  void handleCarTurn()
  {
    // Handle turning smoothly
    if (isTurningLeft)
    {
      transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
      if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
      {
        isTurningLeft = false;  // Stop turning once the target rotation is reached
      }
    }

    if (isTurningRight)
    {
      transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
      if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
      {
        isTurningRight = false;  // Stop turning once the target rotation is reached
      }
    }
  }
  void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Player" || col.gameObject.tag == "wall")
    {
      Destroy(gameObject); // Destroy the car when it hits a player or a wall
    }
  }

  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "leftTurn")
    {
      turnLeft();
      Destroy(col.gameObject); // Destroy the trigger after turning
    }
    else if (col.gameObject.tag == "rightTurn")
    {
      turnRight();
      Destroy(col.gameObject); // Destroy the trigger after turning
    }
  }

  void turnLeft()
  {
    if (!isTurningLeft)
    {
      isTurningLeft = true;
      targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -rotationAngle, 0)); // Rotate 90 degrees to the left
    }
  }

  void turnRight()
  {
    if (!isTurningRight)
    {
      isTurningRight = true;
      targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, rotationAngle, 0)); // Rotate 90 degrees to the right
    }
  }

  void carForward()
  {
    rb.velocity = transform.forward * carSpeed * Time.deltaTime; // Move forward in the current facing direction
  }
  void ApplyDownForce()
  {
    // Apply downward force to keep the car on the ground
    rb.AddForce(-transform.up * downForce, ForceMode.Acceleration);
  }
}
