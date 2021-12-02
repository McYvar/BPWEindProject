using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entered Attacking");
    }


    public override void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("Left Attacking");
    }


    public override void UpdateState(EnemyStateManager enemy)
    {
        if (!enemy.PlayerAttackingCheck())
        {
            enemy.SwitchState(enemy.chaceState);
        }

        enemy.AttackPlayer();
    }


    public override void FixedUpdateState(EnemyStateManager enemy)
    {
    }

}
