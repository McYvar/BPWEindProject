using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : BaseState
{
    Player player;
    Rigidbody rb;

    public GameObject orientation;
    public float playerSpeed;
    public float maxSpeed;

    private float verticalInput;
    private float horizontalInput;

    public override void OnAwake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }


    public override void OnEnter()
    {
        Debug.Log("Entered Airborne");
        Vector3 velocity = rb.velocity;
        velocity.y = 0;
        if (velocity.magnitude < 0.5f)
        {
            velocity.x = 0;
            velocity.z = 0;
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }
    }


    public override void OnExit()
    {
    }


    public override void OnUpdate()
    {
        if (player.onGround && Mathf.Abs(rb.velocity.y) < 0.1f) stateManager.SwitchState(stateManager.lastState.GetType());

        // Input related stuff
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }


    public override void OnFixedUpdate()
    {
        AirMovement();
    }


    private void AirMovement()
    {
        // Cancel out input if the magnitude of the axis gets too high
        float xMagnitude = rb.velocity.x, zMagnitude = rb.velocity.z;
        if (horizontalInput > 0 && xMagnitude > maxSpeed) horizontalInput = 0;
        if (horizontalInput < 0 && xMagnitude < -maxSpeed) horizontalInput = 0;
        if (verticalInput > 0 && zMagnitude > maxSpeed) verticalInput = 0;
        if (verticalInput < 0 && zMagnitude < -maxSpeed) verticalInput = 0;

        rb.AddForce(orientation.transform.forward * verticalInput * playerSpeed, ForceMode.VelocityChange);
        rb.AddForce(orientation.transform.right * horizontalInput * playerSpeed, ForceMode.VelocityChange);
    }

}
