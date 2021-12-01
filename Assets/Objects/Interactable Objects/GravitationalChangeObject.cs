using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalChangeObject : MonoBehaviour, ISwitchable
{
    private Rigidbody rb;
    public GameObject playerCenter;

    RemoveInteractableObjects script;

    public Vector3 location { get; set; }

    public float yScale { get; set; }

    public void Switch(Vector3 location)
    {
        transform.position = location;

        if (Mathf.Abs(playerCenter.transform.eulerAngles.z) == 180)
        {
            playerCenter.transform.rotation = Quaternion.Euler(0, 0, 0);
        } 
        else
        {
            playerCenter.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        Physics.gravity *= -1;
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        yScale = transform.localScale.y;

        script = GameObject.Find("ClearRoom").GetComponent<RemoveInteractableObjects>();
        script.list.Add(this.gameObject);
    }


    private void Update()
    {
        location = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z);
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
