using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundState : PlayerBaseState
{
    private Rigidbody rb;

    public override void EnterState(PlayerStateManager player)
    {
        rb = player.rb;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        float jumpForce = player.jumpForce;

        if (Input.GetButton("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            player.isGrounded = false;
        }

        if (!player.isGrounded) player.SwitchState(player.AirborneState);
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }

}
