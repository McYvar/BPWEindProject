using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class only exists to display the force of the rigidbody of the player, can be removed afterwards
public class DisplayForce : MonoBehaviour
{
    Rigidbody rb;
    bool started = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        started = true;
    }
    private void DrawGizmos()
    {
        if (started)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + rb.velocity);
        }
    }
}
