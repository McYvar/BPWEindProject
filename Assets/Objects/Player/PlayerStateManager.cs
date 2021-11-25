using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerBaseState currentState;
    public PlayerOnGroundState OnGroundState = new PlayerOnGroundState();
    public PlayerAirborneState AirborneState = new PlayerAirborneState();
    public PlayerCrouchState PlayerCrouchState = new PlayerCrouchState();

    public float playerSpeed;
    public float sensitivity;
    public float jumpForce;
    public Rigidbody rb;
    public bool spacePress;

    public GameObject playerCamera;

    private Quaternion cameraRotation;



    void Start()
    {
        currentState = OnGroundState;
        currentState.EnterState(this);
    }

    private void LateUpdate()
    {
        playerCamera.transform.position = transform.position;

        cameraRotation.x += Input.GetAxis("Mouse Y") * -1 * sensitivity;
        cameraRotation.y += Input.GetAxis("Mouse X") * sensitivity;

        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -89.7f, 89.7f);

        playerCamera.transform.localRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y + 90, 0);
    }

    private void Update()
    {
        if (Input.GetButton("Jump")) spacePress = true;
        else spacePress = false;
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
}
