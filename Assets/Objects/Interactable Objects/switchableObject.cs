using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableObject : MonoBehaviour, ISwitchable
{
    PlayerStateManager playerScript;
    public Vector3 location { get; set; }
    public float yScale { get; set; }


    private void Start()
    {
        playerScript = GameObject.FindObjectOfType<PlayerStateManager>();
        yScale = transform.localScale.y;
    }


    private void Update()
    {
        if (playerScript.getCurrentZAxis() == 180)
        {
            location = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * 3/2), transform.position.z);
        } 
        else
        {
            location = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z);
        }
    }

    public void Switch(Vector3 location)
    {
        transform.position = location;
    }

}
