using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
  public float carSpeed = 10f;      // Speed of the car
  public float downForce = 10f;   // Speed of rotation
  private Rigidbody rb;


  void Start()
  {
    rb = GetComponent<Rigidbody>();

  }

  void Update()
  {
    carForward(); // Move the car forward continuously
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

}
