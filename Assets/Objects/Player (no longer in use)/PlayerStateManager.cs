using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStateManager : MonoBehaviour, IDamagable
{
    #region Variables and such
    // Player states
    private PlayerBaseState currentState;
    public PlayerOnGroundState OnGroundState = new PlayerOnGroundState();
    public PlayerAirborneState AirborneState = new PlayerAirborneState();
    public PlayerDeadState deadState = new PlayerDeadState();

    // Assign rigidbody to script
    public Rigidbody rb;

    // Camera and player orientation
    public GameObject orientation;
    public GameObject playerCamera;
    public GameObject playerCenter;
    public float flip;

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

    // Damage and health
    public float minFallVelocityToGainDamage;
    public float fallDamageMultiplier;
    public HealtBar healtBar;
    public int healt { get; set; }
    #endregion


    #region Awake/Start/Update
    private void Awake()
    {
        // Initiate rigidbody with the component
        rb = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        Physics.gravity = new Vector3(Physics.gravity.x, -20, Physics.gravity.z);

        isGrounded = false;

        // Lock and hide the cursor while playing
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Somthing that has to do with the camera
        flip = 1;

        // Initialise the healbar
        healtBar.SetMaxHealth(100);

        // Initiate the first state and enter it
        currentState = AirborneState;
        currentState?.EnterState(this);
    }


    private void Update()
    {
        // Non-Physics stuff will be put in update
        currentState?.UpdateState(this);
        isGrounded = sphereCasting();

        CameraFlip();
    }


    void FixedUpdate()
    {
        // Physics stuff will be put in fixed update
        currentState?.FixedUpdateState(this);
        Movement();
    }
    #endregion


    #region Movement related
    // Subroutine for player input
    public void playerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal") * flip;
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftControl)) CrouchStart();
        if (Input.GetKeyUp(KeyCode.LeftControl)) CrouchStop();

        if (Input.GetMouseButtonDown(0)) onImpact();
        if (Input.GetKeyDown(KeyCode.E)) OnPress();

        if (Input.GetKeyDown(KeyCode.LeftShift)) maxSpeed *= 2;
        if (Input.GetKeyUp(KeyCode.LeftShift)) maxSpeed /= 2;
    }


    // Subroutine for player movement
    private void Movement()
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

        rb.AddForce(orientation.transform.forward * verticalInput * playerSpeed * airStrafe * Time.deltaTime, ForceMode.Force);
        rb.AddForce(orientation.transform.right * horizontalInput * playerSpeed * airStrafe * Time.deltaTime, ForceMode.Force);
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
        transform.position = new Vector3(transform.position.x, transform.position.y - crouchHeight * flip, transform.position.z);
    }


    private void CrouchStop()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + crouchHeight, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y + crouchHeight, transform.position.z);
    }


    public void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;

        Vector3 rotation = playerCamera.transform.localRotation.eulerAngles;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation * flip, yRotation * flip, playerCenter.transform.localEulerAngles.z);
        orientation.transform.localRotation = Quaternion.Euler(0, yRotation * flip, 0);
    }


    public void CameraFlip()
    {
        if (Mathf.Abs(playerCenter.transform.localEulerAngles.z) == 180) flip = -1;
        else flip = 1;
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


    public float getCurrentZAxis()
    {
        return playerCenter.transform.localEulerAngles.z;
    }


    public void flipZAxis(Quaternion rotation)
    {
        playerCenter.transform.rotation = rotation;
    }
    #endregion


    #region Sphere and raycasting
    private bool sphereCasting()
    {
        sphereCastOrigin = (transform.position + (Vector3.up - (Vector3.up * transform.localScale.y)) * flip);
        sphereCastDirection = -transform.up * flip;
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
        Gizmos.DrawSphere(sphereCastOrigin + sphereCastDirection * sphereCastHitDistance, sphereCastRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + rb.velocity);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * 1000);
    }


    private void onImpact()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
        {
            ISwitchable component = hit.collider.GetComponent<ISwitchable>();
            if (component != null)
            {
                Vector3 componentTempLocation = component.location;
                Vector3 currentTempLocation = transform.position;

                if (getCurrentZAxis() == 180)
                {
                    component.Switch(new Vector3(currentTempLocation.x, currentTempLocation.y - transform.localScale.y + (component.yScale * 3/2), currentTempLocation.z));
                    transform.position = new Vector3(componentTempLocation.x, componentTempLocation.y + transform.localScale.y, componentTempLocation.z);
                }
                else
                {
                    component.Switch(new Vector3(currentTempLocation.x, currentTempLocation.y - transform.localScale.y + (component.yScale / 2), currentTempLocation.z));
                    transform.position = new Vector3(componentTempLocation.x, componentTempLocation.y + transform.localScale.y, componentTempLocation.z);
                }
            }
        }

    }


    private void OnPress()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
        {
            IPressable button = hit.collider.GetComponent<IPressable>();
            if (button != null)
            {
                Vector3 buttonLocation = hit.point;
                float dist = Vector3.Distance(transform.position, buttonLocation);
                if (dist < 2.5f)
                {
                    button.PressObject();
                }
            }
        }
    }
    #endregion


    #region Damage
    public bool CheckDead()
    {
        if (healtBar?.getHealth() <= 0)
        {
            return true;
        }
        return false;
    }


    public void fallDamage(float fallVelocity)
    {
        if (Mathf.Abs(fallVelocity) > minFallVelocityToGainDamage)
        {
            int damageTaken = (int) Mathf.Abs((fallVelocity - minFallVelocityToGainDamage) * fallDamageMultiplier);
            takeDamage(damageTaken);
        }
    }


    public void takeDamage(int amount)
    {
        healtBar.setHealth(healtBar.getHealth() - amount);
    }


    public void Dead()
    {
        healtBar.setHealth(0);
        StartCoroutine(DeadCounter());
    }


    private IEnumerator DeadCounter()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion


    #region Collision
    private void OnTriggerStay(Collider collider)
    {
        IPressable component = collider.GetComponent<IPressable>();
        if (component != null) component.PressObject();
        
    }


    private void OnTriggerExit(Collider collider)
    {
        IPressable component = collider.GetComponent<IPressable>();
        if (component != null) component.UnpressObject();
    }
    #endregion


    // Subroutine to switch in between states
    public void SwitchState(PlayerBaseState state)
    {
        currentState?.ExitState(this);
        currentState = state;
        currentState?.EnterState(this);
    }

}
