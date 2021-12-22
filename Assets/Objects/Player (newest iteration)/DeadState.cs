using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
    Player player;
    Rigidbody rb;
    public override void OnAwake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEnter()
    {
        // If the player enters the dead state the body becomes smaller and you can no longer move
        player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y / 2, player.transform.localScale.z);

        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        player.Dead();
    }

    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}
