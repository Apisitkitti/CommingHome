using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class CarMove : MonoBehaviour
{
  public float carSpeed = 0f;
  public float turnSpeed;
  [SerializeField] Rigidbody rb;
  private Vector3 initForward;
  private float rotationAngle;


  void Start()
  {
    rb = GetComponent<Rigidbody>();
    initForward = transform.forward;
    rotationAngle = transform.rotation.y;
  }
  void Update()
  {
    carForward();
    carTurn();
  }

  void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Player")
    {
      Destroy(gameObject);
    }
    else if (col.gameObject.tag == "wall")
    {
      Destroy(gameObject);
    }
  }
  void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "leftTurn")
    {
      StartCoroutine(turnLeft());
      Destroy(col.gameObject);
    }
    if (col.gameObject.tag == "rightTurn")
    {
      StartCoroutine(turnRight());
      Destroy(col.gameObject);
    }
  }
  void carTurn()
  {
    transform.Rotate(0, rotationAngle, 0);
  }
  IEnumerator turnLeft()
  {
    while (rotationAngle % 90 != 0)
      yield return rotationAngle -= turnSpeed;

  }
  IEnumerator turnRight()
  {
    while (rotationAngle % 90 != 0)
      yield return rotationAngle += turnSpeed;
  }
  void carForward()
  {
    rb.velocity = initForward * carSpeed * Time.deltaTime;
  }
}


