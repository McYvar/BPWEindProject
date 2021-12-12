using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    private Player player;

    public GameObject orientation;
    public float playerSpeed;
    public float maxSpeed;
    public float sprintIncreaser;
    public float counterMovement;
    public float crouchSpeedReduction;
    private bool isCrouching;

    public float jumpForce;
    private bool hasJumped;

    private float verticalInput;
    private float horizontalInput;
    private float threshold = 0.01f;
    private Rigidbody rb;

    public override void OnAwake()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
    }


    public override void OnEnter()
    {
        Debug.Log("Entered MoveState");

        hasJumped = false;
    }


    public override void OnExit()
    {
    }


    public override void OnUpdate()
    {
        // Input related stuff
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Jump") && player.onGround) Jump();

        if (!player.onGround)
        {
            stateManager.SwitchState(typeof(AirborneState));
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) 
        {
            maxSpeed /= crouchSpeedReduction;
            isCrouching = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl)) 
        {
            maxSpeed *= crouchSpeedReduction;
            isCrouching = false;
        }

        if (!isCrouching)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) maxSpeed *= sprintIncreaser;
            if (Input.GetKeyUp(KeyCode.LeftShift)) maxSpeed /= sprintIncreaser;
        }
    }


    public override void OnFixedUpdate()
    {
        if (player.onGround) Movement();
    }


    // Subroutine for player movement
    private void Movement()
    {
        // Find velocity that is relative to where the player is looking
        Vector2 magnitude = VelocityRelativeToCameraRotation();

        // Cancel out input if the magnitude of the axis gets too high
        float xMagnitude = magnitude.x, yMagnitude = magnitude.y;
        if (horizontalInput > 0 && xMagnitude > maxSpeed) horizontalInput = 0;
        if (horizontalInput < 0 && xMagnitude < -maxSpeed) horizontalInput = 0;
        if (verticalInput > 0 && yMagnitude > maxSpeed) verticalInput = 0;
        if (verticalInput < 0 && yMagnitude < -maxSpeed) verticalInput = 0;

        // This line has to come after canceling out the axis input otherwise it doesn't work
        if (!hasJumped) CounterMovement(horizontalInput, verticalInput, magnitude);

        rb.AddForce(orientation.transform.forward * verticalInput * playerSpeed, ForceMode.VelocityChange);
        rb.AddForce(orientation.transform.right * horizontalInput * playerSpeed, ForceMode.VelocityChange);
    }


    private void CounterMovement(float horizontal, float vertical, Vector2 magnitude)
    {
        if (Mathf.Abs(magnitude.x) > threshold && Mathf.Abs(horizontal) < 0.05f || (magnitude.x < -threshold && horizontal > 0) || (magnitude.x > threshold && horizontal < 0))
        {
            rb.AddForce(orientation.transform.right * -magnitude.x * counterMovement * playerSpeed, ForceMode.VelocityChange);
        }

        if (Mathf.Abs(magnitude.y) > threshold && Mathf.Abs(vertical) < 0.05f || (magnitude.y < -threshold && vertical > 0) || (magnitude.y > threshold && vertical < 0))
        {
            rb.AddForce(orientation.transform.forward * -magnitude.y * counterMovement * playerSpeed, ForceMode.VelocityChange);
        }
    }


    private Vector2 VelocityRelativeToCameraRotation()
    {
        float cameraRotation = orientation.transform.eulerAngles.y;

        // Unity description for Atan2:
        // Returns the angle in radians whose Tan is y/x.
        // Return value is the angle between the x - axis and a 2D vector starting at zero and terminating at(x, y).
        // So in this case its an angle given in radians between two speed vectors of this rigidbody
        // Source: https://docs.unity3d.com/ScriptReference/Mathf.Atan2.html
        float velocityAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(cameraRotation, velocityAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float yMagnitude = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMagnitude = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMagnitude, yMagnitude);
    }

    
    private void Jump()
    {
        if (!hasJumped && player.onGround)
        {
            // Add jump forces
            rb.AddForce(Vector2.up * jumpForce, ForceMode.VelocityChange);
            hasJumped = true;
        }
    }

}
