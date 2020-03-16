using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.layer == 10)
    {
      Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
      Vector3 velocity = rb.velocity;
      velocity.x = -velocity.x;
      velocity.z = -velocity.z;
      rb.velocity = velocity;
    }
  }
}
