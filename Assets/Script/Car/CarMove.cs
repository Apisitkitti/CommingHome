using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
  public float carSpeed = 10f;      // Speed of the car
  private Rigidbody rb;
  public float downForce = 10f;

  void Start()
  {
    rb = GetComponent<Rigidbody>();

  }
  void FixedUpdate()
  {
    ApplyDownForce();
    carForward();
  }


  void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Player" || col.gameObject.tag == "wall")
    {
      Destroy(gameObject); // Destroy the car when it hits a player or a wall
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
