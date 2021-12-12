using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private FiniteStateMachine finiteStateMachine;
    public bool onGround;

    // Camera and player orientation
    public GameObject orientation;
    public GameObject playerCamera;
    public GameObject playerCenter;

    public float sensitivity;
    private float xRotation; // for camera and for orientation
    private float yRotation; // for camera only

    // Variables for spherecast
    public GameObject sphereCastHitObject;
    private float sphereCastRadius = 0.49f;
    private float sphereCastMaxDistance = 0.65f;
    public LayerMask sphereCastLayerMask;

    private float sphereCastHitDistance;
    private Vector3 sphereCastOrigin;
    private Vector3 sphereCastDirection;

    // Crouching
    public float crouchHeight;
    private float originalScale;
    private float crouchScaleSpeed = 0.05f;


    public void OnAwake()
    {
        finiteStateMachine = new FiniteStateMachine(typeof(MoveState), GetComponents<BaseState>());
    }

    public void OnStart()
    {
        onGround = true;
        originalScale = transform.localScale.y;
    }


    public void OnUpdate()
    {
        CameraRotation();

        finiteStateMachine.OnUpdate();
        sphereCasting();

        if (Input.GetKey(KeyCode.LeftControl)) CrouchStart();
        else CrouchStop();
    }

    public void OnFixedUpdate()
    {
        finiteStateMachine.OnFixedUpdate();
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;

        Vector3 rotation = playerCamera.transform.localRotation.eulerAngles;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, playerCenter.transform.localEulerAngles.z);
        orientation.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }


    private void sphereCasting()
    {
        sphereCastOrigin = (transform.position + (Vector3.up - (Vector3.up * transform.localScale.y)));
        sphereCastDirection = -transform.up;
        RaycastHit sphereCastHit;
        if (Physics.SphereCast(sphereCastOrigin, sphereCastRadius, sphereCastDirection, out sphereCastHit, sphereCastMaxDistance, sphereCastLayerMask, QueryTriggerInteraction.UseGlobal))
        {
            sphereCastHitObject = sphereCastHit.transform.gameObject;
            sphereCastHitDistance = sphereCastHit.distance;
            onGround = true;
        }
        else
        {
            sphereCastHitObject = null;
            sphereCastHitDistance = sphereCastMaxDistance;
            onGround = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(sphereCastOrigin, sphereCastOrigin * sphereCastHitDistance);
        Gizmos.DrawSphere(sphereCastOrigin + sphereCastDirection * sphereCastHitDistance, sphereCastRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * 1000);
    }


    private void CrouchStart()
    {
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Clamp(transform.localScale.y - crouchScaleSpeed, crouchHeight, crouchHeight), transform.localScale.z);
    }


    private void CrouchStop()
    {
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Clamp(transform.localScale.y + crouchScaleSpeed, crouchHeight, originalScale), transform.localScale.z);
    }

}
