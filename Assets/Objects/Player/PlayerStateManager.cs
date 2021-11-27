using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerBaseState currentState;
    public PlayerOnGroundState OnGroundState = new PlayerOnGroundState();
    public PlayerAirborneState AirborneState = new PlayerAirborneState();

    public float playerSpeed;
    public float sensitivity;
    public float jumpForce;
    public float playerCrouch;

    public bool isGrounded;

    public Rigidbody rb;

    public GameObject playerCamera;

    private Quaternion cameraRotation;

    // variables for spherecast
    public GameObject sphereCastHitObject;
    public float sphereCastRadius;
    public float sphereCastMaxDistance;
    public LayerMask sphereCastLayerMast;

    private float sphereCastHitDistance;
    private Vector3 sphereCastOrigin;
    private Vector3 sphereCastDirection;

    void Start()
    {
        currentState = AirborneState;
        currentState.EnterState(this);
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
    }

    private void LateUpdate()
    {
        playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + (0.3f * -playerCrouch) + 0.5f, transform.position.z);

        cameraRotation.x += -Input.GetAxis("Mouse Y") * sensitivity;
        cameraRotation.y += Input.GetAxis("Mouse X") * sensitivity;

        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -89.7f, 89.7f);

        playerCamera.transform.localRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y + 90, 0);
    }

    private void Update()
    {
        isGrounded = sphereCasting();

        rb.rotation = Quaternion.Euler(0, playerCamera.transform.localEulerAngles.y, 0);
    }


    void FixedUpdate()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    bool sphereCasting()
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
    }
}
