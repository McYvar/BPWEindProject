using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableObject : MonoBehaviour, ISwitchable
{
    public Vector3 location { get; set; }
    public float yScale { get; set; }


    private void Start()
    {
        yScale = transform.localScale.y;
    }


    private void Update()
    {
        location = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y/2), transform.position.z);
    }

    public void Switch(Vector3 location)
    {
        transform.position = location;
    }

}
