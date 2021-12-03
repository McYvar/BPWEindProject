using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerBaseState
{
    private Rigidbody rb;
    private float maxFallingVelocity;

    public override void EnterState(PlayerStateManager player)
    {
        // Re-assign the player rigidbody to funcion overhere
        rb = player.rb;
        maxFallingVelocity = 0;
    }


    public override void ExitState(PlayerStateManager player)
    {
        player.fallDamage(maxFallingVelocity);
    }


    public override void UpdateState(PlayerStateManager player)
    {
        player.playerInput();
        player.CameraRotation();
        // Updates the vertical speed till the player has landed (for damage detection later on)
        if (Mathf.Abs(player.playerCenter.transform.localEulerAngles.z) == 180)
        {
            if (maxFallingVelocity < rb.velocity.y && rb.velocity.y > 0) maxFallingVelocity = rb.velocity.y;
        }
        else
        {
            if (maxFallingVelocity > rb.velocity.y && rb.velocity.y < 0) maxFallingVelocity = rb.velocity.y;
        }

        // If airborne the movementspeed in air is reduced
        if (!player.isGrounded)
        {
            player.airStrafe = 0.1f;
        }

        // If the player lands on a floor then we switch back to the grounded state
        if (player.isGrounded)
        {
            player.SwitchState(player.OnGroundState);
        }
    }


    public override void FixedUpdateState(PlayerStateManager player)
    {
    }


    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }

}
