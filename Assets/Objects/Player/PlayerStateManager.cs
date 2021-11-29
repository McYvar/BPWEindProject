using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    // Player states
    public PlayerBaseState currentState;
    public PlayerOnGroundState OnGroundState = new PlayerOnGroundState();
    public PlayerAirborneState AirborneState = new PlayerAirborneState();

    // Assign rigidbody to script
    public Rigidbody rb;

    // Gameobject to perform certain actions on or with
    public GameObject orientation;
    public GameObject playerCamera;

    // Variables for spherecast
    public GameObject sphereCastHitObject;
    public float sphereCastRadius;
    public float sphereCastMaxDistance;
    public LayerMask sphereCastLayerMast;

    private float sphereCastHitDistance;
    private Vector3 sphereCastOrigin;
    private Vector3 sphereCastDirection;

    // Input
    private float horizontalInput;
    private float verticalInput;

    // Movement
    public float playerSpeed;
    public float maxSpeed;
    public float counterMovement;
    public float jumpForce;
    public float airStrafe;
    public bool isGrounded;
    private float threshold = 0.01f;

    // Variables for camera rotating
    public float sensitivity;
    private float xRotation; // for camera and for orientation
    private float yRotation; // for camera only

    // Crouching
    public float crouchHeight = 0.5f;
    public float crouchSpeedReduction;

    void Start()
    {
        // Initiate the first state and enter it
        currentState = AirborneState;
        currentState.EnterState(this);

        // Initiate rigidbody with the component
        rb = GetComponent<Rigidbody>();
        isGrounded = false;

        // Lock and hide the cursor while playing
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        // Non-Physics stuff will be put in update
        currentState.UpdateState(this);
        CameraRotation();
        playerInput();
        isGrounded = sphereCasting();
    }


    void FixedUpdate()
    {
        // Physics stuff will be put in fixed update
        currentState.FixedUpdateState(this);
        Movement();
    }


    public void OnCollisionEnter(Collision collision)
    {
        // Collision in current state will be detected aswell (not that it has any functionality so far but it works)
        currentState.OnCollisionEnter(this, collision);
    }


    // Subroutine to switch in between states
    public void SwitchState(PlayerBaseState state)
    {
        state.ExitState(this);
        currentState = state;
        state.EnterState(this);
    }


    // Subroutine for player input
    private void playerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftControl)) CrouchStart();
        if (Input.GetKeyUp(KeyCode.LeftControl)) CrouchStop();

        if (Input.GetMouseButtonDown(0)) onImpact();
    }


    // Subroutine for player movement
    public void Movement()
    {
        // Find velocity that is relative to where the player is looking
        Vector2 magnitude = VelocityRelativeToCameraRotation();
        CounterMovement(horizontalInput, verticalInput, magnitude);

        // Cancel out input if the magnitude of the axis gets too high
        float xMagnitude = magnitude.x, yMagnitude = magnitude.y;
        if (horizontalInput > 0 && xMagnitude > maxSpeed) horizontalInput = 0;
        if (horizontalInput < 0 && xMagnitude < -maxSpeed) horizontalInput = 0;
        if (verticalInput > 0 && yMagnitude > maxSpeed) verticalInput = 0;
        if (verticalInput < 0 && yMagnitude < -maxSpeed) verticalInput = 0;

        rb.AddForce(orientation.transform.forward * verticalInput * playerSpeed * airStrafe * crouchSpeedReduction * Time.deltaTime, ForceMode.Force);
        rb.AddForce(orientation.transform.right * horizontalInput * playerSpeed * airStrafe * crouchSpeedReduction * Time.deltaTime, ForceMode.Force);
    }

    private void CounterMovement(float horizontal, float vertical, Vector2 magnitude)
    {
        if (!isGrounded) return;
        if (Mathf.Abs(magnitude.x) > threshold && Mathf.Abs(horizontal) < 0.05f || (magnitude.x < -threshold && horizontal > 0) || (magnitude.x > threshold && horizontal < 0))
        {
            rb.AddForce(orientation.transform.right * -magnitude.x * counterMovement, ForceMode.VelocityChange);
        }

        if (Mathf.Abs(magnitude.y) > threshold && Mathf.Abs(vertical) < 0.05f || (magnitude.y < -threshold && vertical > 0) || (magnitude.y > threshold && vertical < 0))
        {
            rb.AddForce(orientation.transform.forward * -magnitude.y * counterMovement, ForceMode.VelocityChange);
        }
    }


    private void CrouchStart()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - crouchHeight, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - crouchHeight, transform.position.z);
    }


    private void CrouchStop()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + crouchHeight, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y + crouchHeight, transform.position.z);
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;

        Vector3 rotation = playerCamera.transform.localRotation.eulerAngles;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
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

    private bool sphereCasting()
    {
        sphereCastOrigin = transform.position;
        sphereCastDirection = -transform.up;
        RaycastHit sphereCastHit;
        if (Physics.SphereCast(sphereCastOrigin, sphereCastRadius, sphereCastDirection, out sphereCastHit, sphereCastMaxDistance, sphereCastLayerMast, QueryTriggerInteraction.UseGlobal))
        {
            sphereCastHitObject = sphereCastHit.transform.gameObject;
            sphereCastHitDistance = sphereCastHit.distance;
            return true;
        }
        else
        {
            sphereCastHitObject = null;
            sphereCastHitDistance = sphereCastMaxDistance;
            return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(sphereCastOrigin, sphereCastOrigin * sphereCastHitDistance);
        Gizmos.DrawSphere(sphereCastOrigin + sphereCastDirection * sphereCastHitDistance, sphereCastRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + rb.velocity);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * 1000);
    }


    private void onImpact()
    {
        RaycastHit hit;
        if (Physics.Raycast(orientation.transform.position, playerCamera.transform.forward, out hit))
        {
            ISwitchable component = hit.collider.GetComponent<ISwitchable>();
            if (component != null)
            {
                Vector3 componentTempLocation = component.location;
                Vector3 currentTempLocation = transform.position;

                component.Switch(new Vector3(currentTempLocation.x, currentTempLocation.y - transform.localScale.y + (component.yScale/2), currentTempLocation.z));
                transform.position = new Vector3(componentTempLocation.x, componentTempLocation.y + transform.localScale.y, componentTempLocation.z);
            }
        }
    }

}
