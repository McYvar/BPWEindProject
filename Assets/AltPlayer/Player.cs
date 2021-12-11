using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private FiniteStateMachine finiteStateMachine;
    private BaseState currentState;
    private bool onGround;

    // Camera and player orientation
    public GameObject orientation;
    public GameObject playerCamera;
    public GameObject playerCenter;

    public float sensitivity;
    private float xRotation; // for camera and for orientation
    private float yRotation; // for camera only

    // Variables for spherecast
    public GameObject sphereCastHitObject;
    public float sphereCastRadius;
    public float sphereCastMaxDistance;
    public LayerMask sphereCastLayerMask;

    private float sphereCastHitDistance;
    private Vector3 sphereCastOrigin;
    private Vector3 sphereCastDirection;


    public void OnAwake()
    {
        finiteStateMachine = new FiniteStateMachine(typeof(IdleState), GetComponents<BaseState>());
    }

    public void OnStart()
    {
        onGround = true;
    }


    public void OnUpdate()
    {
        CameraRotation();

        finiteStateMachine.OnUpdate();

        if (!sphereCasting() && onGround)
        {
            currentState = finiteStateMachine.getCurrentState();
            finiteStateMachine?.SwitchState(typeof(AirborneState));
            onGround = false;
        }
        else if (sphereCasting() && !onGround)
        {
            finiteStateMachine?.SwitchState(currentState?.GetType());
            onGround = true;
        }
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


    private bool sphereCasting()
    {
        sphereCastOrigin = (transform.position + (Vector3.up - (Vector3.up * transform.localScale.y)));
        sphereCastDirection = -transform.up;
        RaycastHit sphereCastHit;
        if (Physics.SphereCast(sphereCastOrigin, sphereCastRadius, sphereCastDirection, out sphereCastHit, sphereCastMaxDistance, sphereCastLayerMask, QueryTriggerInteraction.UseGlobal))
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(sphereCastOrigin, sphereCastOrigin * sphereCastHitDistance);
        Gizmos.DrawSphere(sphereCastOrigin + sphereCastDirection * sphereCastHitDistance, sphereCastRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * 1000);
    }

}
