using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAirborneState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entered Airborne");
        enemy.enemy.enabled = false;
        enemy.DisableConstrains();
    }


    public override void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("Left Airborne");
        enemy.enemy.enabled = true;
        enemy.EnableConstrains();
    }


    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.grounded)
        {
            enemy.SwitchState(enemy.previousState);
        }
    }


    public override void FixedUpdateState(EnemyStateManager enemy)
    {
    }

}
