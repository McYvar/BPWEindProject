using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class velocityClamp : MonoBehaviour
{
    private Rigidbody rb;
    public float clamp;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, clamp);
    }
}
