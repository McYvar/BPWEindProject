using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void OnAwake()
    {
    }


    public override void OnEnter()
    {
        Debug.Log("Entered Idlestate");
        StartCoroutine(USleepin());
    }


    public override void OnExit()
    {
        StopAllCoroutines();
    }


    public override void OnUpdate()
    {
        if (Input.anyKey) stateManager.SwitchState(typeof(MoveState));
    }


    public override void OnFixedUpdate()
    {
    }


    private IEnumerator USleepin()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("U aslep?");
    }
}
