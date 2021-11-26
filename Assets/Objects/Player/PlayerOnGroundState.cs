using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundState : PlayerBaseState
{
    private Rigidbody rb;

    public override void EnterState(PlayerStateManager player) {
        rb = player.rb;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        float jumpForce = player.jumpForce;
        float speed = player.playerSpeed;
        float crouch = 1 - (0.5f * player.playerCrouch);

        float verticalMovement = Input.GetAxis("Vertical") * speed * crouch;
        float horizontalMovement = Input.GetAxis("Horizontal") * speed * crouch;

        rb.AddForce(rb.transform.forward * verticalMovement, ForceMode.Force);

        if (player.spacePress)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            player.SwitchState(player.AirborneState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }
}
