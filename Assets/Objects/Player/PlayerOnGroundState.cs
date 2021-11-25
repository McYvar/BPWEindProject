using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player) {
        Debug.Log("test");
    }

    public override void UpdateState(PlayerStateManager player) { }

}
