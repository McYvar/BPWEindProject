using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwitchableEnemy : MonoBehaviour, ISwitchable
{
    public Vector3 location { get; set; }
    public float yScale { get; set; }

    private NavMeshAgent enemy;
    private Rigidbody rb;


    private void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        yScale = transform.localScale.y;
    }


    private void Update()
    {
        location = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z);
    }

    public void Switch(Vector3 location)
    {
        enemy.enabled = false;
        transform.position = location;
        enemy.enabled = true;
    }

}
