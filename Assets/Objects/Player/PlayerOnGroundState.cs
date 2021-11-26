using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundState : PlayerBaseState
{
    private Rigidbody rb;
    float speed;

    public override void EnterState(PlayerStateManager player)
    {
        rb = player.rb;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        float jumpForce = player.jumpForce;
        speed = player.playerSpeed;
        float crouch = 1 - (0.5f * player.playerCrouch);

        Vector3 velocity = rb.velocity;
        if (Input.GetAxis("Vertical") != 1 || Input.GetAxis("Vertical") != -1)
        {
            velocity.z = 0;
        }

        if (Input.GetAxis("Horizontal") != 1 || Input.GetAxis("Horizontal") != -1)
        {
            velocity.x = 0;
        }

        if (Input.GetKey(KeyCode.W)) rb.AddForce(rb.transform.forward * speed * crouch, ForceMode.VelocityChange);
        if (Input.GetKey(KeyCode.S)) rb.AddForce(rb.transform.forward * -speed * crouch, ForceMode.VelocityChange);
        if (Input.GetKey(KeyCode.D)) rb.AddForce(rb.transform.right * speed * crouch, ForceMode.VelocityChange);
        if (Input.GetKey(KeyCode.A)) rb.AddForce(rb.transform.right * -speed * crouch, ForceMode.VelocityChange);
        rb.velocity = velocity;

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
