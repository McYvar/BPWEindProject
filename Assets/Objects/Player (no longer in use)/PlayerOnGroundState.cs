using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundState : PlayerBaseState
{
    private Rigidbody rb;

    public override void EnterState(PlayerStateManager player)
    {
        if (player.CheckDead())
        {
            player.SwitchState(player.deadState);
        }

        // Re-assign the player rigidbody to funcion overhere
        rb = player.rb;

        // If the player is in this state (on the ground) then the player can move at normal speed again
        player.airStrafe = 1;
    }


    public override void ExitState(PlayerStateManager player)
    {
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.playerInput();
        player.CameraRotation();
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        // Gain the jumpforce from the main class and store as local variable here
        float jumpForce = player.jumpForce;

        // If the player uses the jump butten (space in this case) then-
        if (Input.GetButton("Jump"))
        {
            // First reset the y velocity so each jump is always the same upwards velocity
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            // -a certain amount of force gets added so you jump
            rb.AddForce(Vector3.up * jumpForce * player.flip, ForceMode.VelocityChange);
            player.isGrounded = false;
        }

        // If the player is not on the ground then of course we switch to a different state
        if (!player.isGrounded) player.SwitchState(player.AirborneState);
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }

}
