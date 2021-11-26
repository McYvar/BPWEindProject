using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player) {
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
