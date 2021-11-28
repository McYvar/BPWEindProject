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
        rb.useGravity = false;

        float jumpForce = player.jumpForce;

        player.Movement();

        if (Input.GetButton("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            player.isGrounded = false;
        }

        if (!player.isGrounded) player.SwitchState(player.AirborneState);
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }

}
