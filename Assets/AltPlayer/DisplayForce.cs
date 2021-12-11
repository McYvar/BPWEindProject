using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayForce : MonoBehaviour
{
    Rigidbody rb;
    bool started = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        started = true;
    }
    private void OnDrawGizmos()
    {
        if (started)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + rb.velocity);
        }
    }
}
