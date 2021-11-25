using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerBaseState
{
    private Rigidbody rb;
    public override void EnterState(PlayerStateManager player) {
        Debug.Log("You just jumped");
        rb = player.rb;
    }

    public override void UpdateState(PlayerStateManager player) {

    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Ground"))
        {
            player.SwitchState(player.OnGroundState);
        }
    }

}
