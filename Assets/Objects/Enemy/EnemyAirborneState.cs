using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAirborneState : EnemyBaseState
{
    private Rigidbody rb;
    private float maxFallingVelocity;
    private float timer;

    public override void EnterState(EnemyStateManager enemy)
    {
        // Disable the navagent and disable rigidbody constrains so the enemy can use physics
        enemy.enemy.enabled = false;
        enemy.DisableConstrains();

        rb = enemy.rb;
        timer = 1;
    }


    public override void ExitState(EnemyStateManager enemy)
    {
        // Enable the navagent and enable rigidbody constrains so the enemy can use AI again
        enemy.EnableConstrains();
        enemy.enemy.enabled = true;

        // Passes max speed when dropping to the ground
        enemy.fallDamage(Mathf.Abs(maxFallingVelocity));
    }


    public override void UpdateState(EnemyStateManager enemy)
    {
        // Updates the vertical speed till the player has landed for passing max velocity while falling
        if (Mathf.Abs(enemy.enemyCenter.transform.localEulerAngles.z) == 180)
        {
            if (maxFallingVelocity < rb.velocity.y && rb.velocity.y > 0) maxFallingVelocity = rb.velocity.y;
            if (rb.velocity.y < 0) maxFallingVelocity = 0;
        }
        else
        {
            if (maxFallingVelocity > rb.velocity.y && rb.velocity.y < 0) maxFallingVelocity = rb.velocity.y;
            if (rb.velocity.y > 0) maxFallingVelocity = 0;
        }

        // Switch back to the previous state if the enemy is back on the ground
        if (enemy.isGrounded && timer < 0)
        {
            enemy.SwitchState(enemy.previousState);
        }

        timer -= Time.deltaTime;
    }


    public override void FixedUpdateState(EnemyStateManager enemy)
    {
    }

}
