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
        rb.MoveRotation(Quaternion.Euler(0, player.playerCamera.transform.localEulerAngles.y, 0));
        rb.MovePosition(rb.position + (player.transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime));
        rb.MovePosition(rb.position + (player.transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime));

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
