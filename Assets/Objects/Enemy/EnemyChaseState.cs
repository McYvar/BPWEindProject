using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
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

        // If the player comes in detection range, the enemy starts attacking the player
        if (enemy.PlayerAttackingCheck())
        {
            enemy.SwitchState(enemy.attackState);
        }

        // If the player drops out of detection range, the enemy stops following the player
        if (!enemy.PlayerDetectionCheck())
        {
            enemy.SwitchState(enemy.idleState);
        }

        enemy.ChacePlayer();
    }


    public override void FixedUpdateState(EnemyStateManager enemy)
    {
    }

}
