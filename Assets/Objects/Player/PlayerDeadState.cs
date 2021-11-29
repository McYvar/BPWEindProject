using System.Collections;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.playerSpeed = 0;
        player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y / 3, player.transform.localScale.z);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - player.transform.localScale.y / 3, player.transform.position.z);

        player.rb.velocity = new Vector3(0, player.rb.velocity.y, 0);

        player.Dead();
    }


    public override void ExitState(PlayerStateManager player) 
    {
    }


    public override void UpdateState(PlayerStateManager player) 
    {
    }


    public override void FixedUpdateState(PlayerStateManager player) 
    {
    }


    public override void OnCollisionEnter(PlayerStateManager player, Collision collision) 
    {
    }

}
