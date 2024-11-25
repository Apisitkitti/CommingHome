using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gley.TrafficSystem;
public class DestroyCar : MonoBehaviour
{
     void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Player" || col.gameObject.tag == "Car"|| col.gameObject.tag == "Bus")
    {
        API.RemoveVehicle(col.gameObject);
    }
  }
   

}
