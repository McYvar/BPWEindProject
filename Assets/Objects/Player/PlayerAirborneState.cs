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
        speed = player.playerSpeed / 1.5f;

        if (player.isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            player.SwitchState(player.OnGroundState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }
}
