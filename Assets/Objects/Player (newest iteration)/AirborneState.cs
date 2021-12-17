using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : BaseState
{
    #region Variables and stuff
    private Player player;
    private Rigidbody rb;

    public GameObject orientation;
    public float playerSpeed;
    public float maxSpeed;

    private float verticalInput;
    private float horizontalInput;

    public float maxFallingVelocity;

    private float flip;
    #endregion


    #region Awake/Start/Update/FixedUpdate
    public override void OnAwake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }


    public override void OnEnter()
    {
        Vector3 velocity = rb.velocity;
        velocity.y = 0;
        if (velocity.magnitude < 0.5f)
        {
            velocity.x = 0;
            velocity.z = 0;
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }

        maxFallingVelocity = 0;
        flip = player.flip;
    }


    public override void OnExit()
    {
        player.fallDamage(Mathf.Abs(maxFallingVelocity));
    }


    public override void OnUpdate()
    {
        // Updates the vertical speed till the player has landed
        if (Mathf.Abs(player.playerCenter.transform.localEulerAngles.z) == 180)
        {
            if (maxFallingVelocity < rb.velocity.y && rb.velocity.y > 0) maxFallingVelocity = rb.velocity.y;
        }
        else
        {
            if (maxFallingVelocity > rb.velocity.y && rb.velocity.y < 0) maxFallingVelocity = rb.velocity.y;
        }

        if (player.onGround && Mathf.Abs(rb.velocity.y) < 0.1f) stateManager.SwitchState(stateManager.lastState.GetType());

        // Input related stuff
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal") * flip;
    }

    public override void OnFixedUpdate()
    {
        AirMovement();
    }
    #endregion


    #region Airmovement
    private void AirMovement()
    {
        Vector3 velocity = rb.velocity;
        velocity.y = 0;
        if (velocity.magnitude > maxSpeed)
        {
            verticalInput = 0;
            horizontalInput = 0;
        }

        rb.AddForce(orientation.transform.forward * verticalInput * playerSpeed, ForceMode.VelocityChange);
        rb.AddForce(orientation.transform.right * horizontalInput * playerSpeed, ForceMode.VelocityChange);
    }
    #endregion
}
