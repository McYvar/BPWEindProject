using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAirborneState : EnemyBaseState
{
    private Rigidbody rb;
    private float maxFallingVelocity;

    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.enemy.enabled = false;
        enemy.DisableConstrains();

        rb = enemy.rb;
    }


    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.EnableConstrains();
        enemy.enemy.enabled = true;

        enemy.fallDamage(maxFallingVelocity);
    }


    public override void UpdateState(EnemyStateManager enemy)
    {
        // Updates the vertical speed till the player has landed (for damage detection later on)
        if (Mathf.Abs(enemy.enemyCenter.transform.localEulerAngles.z) == 180)
        {
            if (maxFallingVelocity < rb.velocity.y && rb.velocity.y > 0) maxFallingVelocity = rb.velocity.y;
        }
        else
        {
            if (maxFallingVelocity > rb.velocity.y && rb.velocity.y < 0) maxFallingVelocity = rb.velocity.y;
        }

        if (enemy.isGrounded)
        {
            enemy.SwitchState(enemy.previousState);
        }
    }


    public override void FixedUpdateState(EnemyStateManager enemy)
    {
    }

}
