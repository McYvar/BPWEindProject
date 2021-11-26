using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundState : PlayerBaseState
{
    private float speed;
    private float jumpForce;

    private Rigidbody rb;

    public override void EnterState(PlayerStateManager player) {
        rb = player.rb;

        speed = player.playerSpeed;
        jumpForce = player.jumpForce;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        float crouch = 1 - (0.5f * player.playerCrouch);
        //rb.MovePosition(rb.position + (player.transform.forward * Input.GetAxis("Vertical") * speed * crouch * Time.deltaTime));
        //rb.MovePosition(rb.position + (player.transform.right * Input.GetAxis("Horizontal") * speed * crouch * Time.deltaTime));
        rb.AddForce(player.transform.right * Input.GetAxis("Horizontal") * speed * crouch);
        rb.AddForce(player.transform.forward * Input.GetAxis("Vertical") * speed * crouch);

        if (player.spacePress)
        {
            rb.AddForce(Vector3.up * jumpForce);
            player.SwitchState(player.AirborneState);
        }

    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
    }

}
