using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTurn : MonoBehaviour
{
  private Quaternion targetRotation;
  public float turnSpeed = 100f;
  private float rotationAngle = 90f;
  private bool isTurningLeft = false;
  private bool isTurningRight = false;
  // Angle to turn
  private Rigidbody rb;
  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update()
  {

    handleCarTurn();
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
  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "leftTurn")
    {
      turnLeft();

    }
    else if (col.gameObject.tag == "rightTurn")
    {
      turnRight();

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

}
