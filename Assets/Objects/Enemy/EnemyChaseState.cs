using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entered Chase");
    }


    public override void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("Left Chase");
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
