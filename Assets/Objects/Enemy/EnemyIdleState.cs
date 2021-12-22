using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
    }


    public override void ExitState(EnemyStateManager enemy)
    {
    }


    public override void UpdateState(EnemyStateManager enemy)
    {
        // Check if the enemy is not on the ground or if its switching, if so, then change state to airborne
        if (!enemy.isGrounded || enemy.switching) enemy.SwitchAndRememberLastState(enemy.airborneState, this);

        // If the player comes in detection range, the enemy starts following the player
        if (enemy.PlayerDetectionCheck())
        {
            enemy.SwitchState(enemy.chaceState);
        }
    }


    public override void FixedUpdateState(EnemyStateManager enemy)
    {
    }

}
