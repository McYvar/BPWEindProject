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


    public override void ExitState(PlayerStateManager player)
    {
    }


    public override void UpdateState(PlayerStateManager player)
    {
        // If airborne the movementspeed in air is reduced
        if (!player.isGrounded)
        {
            player.airStrafe = 0.1f;
        }
    }

    public override void FixedUpdateState(PlayerStateManager player) {
        speed = player.playerSpeed / 1.5f;

        if (player.isGrounded)
        {
            player.SwitchState(player.OnGroundState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }
}
