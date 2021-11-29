using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableObject : MonoBehaviour, ISwitchable
{
    Rigidbody rb;
    public Vector3 location { get; set; }

    public void Switch(Vector3 location)
    {
        transform.position = location;
    }

    public float yScale { get; set; }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        yScale = transform.localScale.y;
    }


    private void Update()
    {
        location = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y/2), transform.position.z);
    }


    private void FixedUpdate()
    {
        float slowDown = 1;
        Vector3 velocity = rb.velocity;

        if (rb.velocity.x > 0) velocity.x = rb.velocity.x - slowDown * Time.deltaTime;
        if (rb.velocity.x < 0) velocity.x = rb.velocity.x + slowDown * Time.deltaTime;
        if (rb.velocity.z > 0) velocity.z = rb.velocity.z - slowDown * Time.deltaTime;
        if (rb.velocity.z < 0) velocity.z = rb.velocity.z + slowDown * Time.deltaTime;

        if (rb.rotation.eulerAngles.y % 90f == 0) rb.rotation = Quaternion.Euler(0, 0, 0);
        

        rb.velocity = velocity;
    }
}
