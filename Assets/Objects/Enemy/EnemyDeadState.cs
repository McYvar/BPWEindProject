using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.enemy.enabled = false;
        enemy.DisableConstrains();
        enemy.Dead();
    }


    public override void ExitState(EnemyStateManager enemy)
    {
    }


    public override void UpdateState(EnemyStateManager enemy)
    {
    }


    public override void FixedUpdateState(EnemyStateManager enemy)
    {
    }

}
