using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundState : PlayerBaseState
{
    private Rigidbody rb;
    float speed;
    float crouch;

    public override void EnterState(PlayerStateManager player)
    {
        rb = player.rb;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        float jumpForce = player.jumpForce;
        speed = player.playerSpeed;
        crouch = 1 - (0.5f * Input.GetAxis("Crouch"));

        movePlayer();

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

    void movePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 velocity = rb.velocity;

        if (verticalInput != 1 || verticalInput != -1) velocity.z = 0;
        if (horizontalInput != 1 || horizontalInput != -1) velocity.x = 0;

        rb.AddForce(rb.transform.forward * verticalInput * speed * crouch, ForceMode.VelocityChange);
        rb.AddForce(rb.transform.right * horizontalInput * speed * crouch, ForceMode.VelocityChange);

        rb.velocity = velocity;
    }
}
