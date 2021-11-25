using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState currentState;
    PlayerOnGroundState OnGroundState = new PlayerOnGroundState();
    PlayerAirborneState AirborneState = new PlayerAirborneState();
    PlayerCrouchState PlayerCrouchState = new PlayerCrouchState();

    // Start is called before the first frame update
    void Start()
    {
        currentState = OnGroundState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
