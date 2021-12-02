using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entered Idle");
    }


    public override void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("Left Idle");
    }


    public override void UpdateState(EnemyStateManager enemy)
    {
        if (!enemy.grounded) enemy.SwitchAndRememberLastState(enemy.airborneState, this);

        if (enemy.PlayerDetectionCheck())
        {
            enemy.SwitchState(enemy.chaceState);
        }
    }


    public override void FixedUpdateState(EnemyStateManager enemy)
    {
    }

}
