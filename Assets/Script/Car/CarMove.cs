using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarMove : MonoBehaviour
{
  public float carSpeed = 0f;
  [SerializeField] Rigidbody rb;
  private Vector3 initForward;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    initForward = transform.forward;
  }
  void Update()
  {
    rb.velocity = initForward * carSpeed * Time.deltaTime;

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
}
