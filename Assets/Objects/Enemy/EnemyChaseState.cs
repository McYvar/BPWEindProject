using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
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
        if (enemy.PlayerAttackingCheck())
        {
            enemy.SwitchState(enemy.attackState);
        }

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
