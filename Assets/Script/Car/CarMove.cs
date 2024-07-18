using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarMove : MonoBehaviour
{
  [SerializeField] float carSpeed = 0f;
  [SerializeField] Rigidbody rb;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }
  void Update()
  {
    rb.velocity = transform.forward * carSpeed * Time.deltaTime;
  }

}