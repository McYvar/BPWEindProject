using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        if (enemy.CheckDead())
        {
            enemy.SwitchState(enemy.deadState);
        }
    }


    public override void ExitState(EnemyStateManager enemy)
    {
    }


    public override void UpdateState(EnemyStateManager enemy)
    {
        if (!enemy.isGrounded) enemy.SwitchAndRememberLastState(enemy.airborneState, this);

        if (enemy.PlayerDetectionCheck())
        {
            enemy.SwitchState(enemy.chaceState);
        }
    }


    public override void FixedUpdateState(EnemyStateManager enemy)
    {
    }

}
