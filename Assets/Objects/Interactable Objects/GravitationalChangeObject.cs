using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalChangeObject : MonoBehaviour, ISwitchable
{
    private Rigidbody rb;

    RemoveInteractableObjects script;
    Player player;

    public Vector3 location { get; set; }

    public float yScale { get; set; }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        script = GameObject.FindObjectOfType<RemoveInteractableObjects>();
        script.list.Add(this.gameObject);

        player = GameObject.FindObjectOfType<Player>();
    }


    private void Start()
    {
        yScale = transform.localScale.y;
    }


    private void Update()
    {
        if (player.getCurrentZAxis() == 180)
        {
            location = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * 3 / 2), transform.position.z);
        }
        else
        {
            location = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z);
        }
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


    public void Switch(Vector3 location)
    {
        transform.position = location;

        if (Mathf.Abs(player.getCurrentZAxis()) == 180)
        {
            player.flipZAxis(Quaternion.Euler(0, 0, 0));
        }
        else
        {
            player.flipZAxis(Quaternion.Euler(0, 0, 180));
        }
        Physics.gravity *= -1;
    }
}
