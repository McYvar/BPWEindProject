using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerBaseState
{
    private Rigidbody rb;
    float speed;
    float crouch;

    public override void EnterState(PlayerStateManager player)
    {
        rb = player.rb;
    }

    public override void UpdateState(PlayerStateManager player) {
        rb.useGravity = true;

        speed = player.playerSpeed / 1.5f;

        airStrafe();
        if (player.isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            player.SwitchState(player.OnGroundState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }

    void airStrafe()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 velocity = rb.velocity;
        if (Mathf.Abs(rb.velocity.z) < 2) rb.AddForce(rb.transform.forward * verticalInput * speed);
        if (Mathf.Abs(rb.velocity.x) < 2) rb.AddForce(rb.transform.right * horizontalInput * speed);

        rb.velocity = velocity;
    }
}
