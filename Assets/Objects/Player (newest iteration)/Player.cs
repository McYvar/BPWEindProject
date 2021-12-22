using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamagable
{
    #region Variables and stuff
    private FiniteStateMachine finiteStateMachine;
    public bool onGround;
    private bool collisionOnGround;

    // Camera and player orientation
    public GameObject orientation;
    public GameObject playerCamera;
    public GameObject playerCenter;
    public int flip;

    public float sensitivity;
    private float xRotation; // for camera and for orientation
    private float yRotation; // for camera only

    // Variables for spherecast
    public GameObject sphereCastHitObject;
    private float sphereCastRadius = 0.49f;
    private float sphereCastMaxDistance = 0.8f;
    public LayerMask sphereCastLayerMask;

    private float sphereCastHitDistance;
    private Vector3 sphereCastOrigin;
    private Vector3 sphereCastDirection;

    // Crouching
    public float crouchHeight;
    private float originalScale;
    private float crouchScaleSpeed = 0.05f;

    // Damage and health
    public float minFallVelocityToGainDamage;
    private float fallDamageMultiplier = 10;
    public HealtBar healtBar;
    public int healt { get; set; }
    private bool tempDeadCheck = false;
    #endregion


    #region Awake/Start/Update/FixedUpdate
    public void Awake()
    {
        finiteStateMachine = new FiniteStateMachine(typeof(MoveState), GetComponents<BaseState>());
    }

    public void Start()
    {
        onGround = true;
        originalScale = transform.localScale.y;
        
        healtBar.SetMaxHealth(100);

        flip = 1;
    }


    public void Update()
    {
        CameraFlip();

        // If the player dies certain parts of the script stop running
        if (!CheckDead())
        {
            tempDeadCheck = false;
            CameraRotation();

            if (sphereCasting())
            {
                if (collisionOnGround) onGround = true;
                else onGround = false;
            }
            else
            {
                onGround = false;
                collisionOnGround = false;
            }
            Input();
        }
        // And if the player actually dies then switch to the dead state
        else if (CheckDead() && !tempDeadCheck)
        {
            tempDeadCheck = true;
            finiteStateMachine.SwitchState(typeof(DeadState));
        }

        finiteStateMachine.OnUpdate();

    }

    public void FixedUpdate()
    {
        finiteStateMachine.OnFixedUpdate();
    }
    #endregion


    #region Input
    // Subroutine for checking input by the player
    private void Input()
    {
        if (UnityEngine.Input.GetKey(KeyCode.LeftControl)) CrouchStart();
        else CrouchStop();

        if (UnityEngine.Input.GetMouseButtonDown(0)) onImpact();
        if (UnityEngine.Input.GetKeyDown(KeyCode.E)) OnPress();
    }
    #endregion


    #region Crouching
    // Subroutine for startcrouching
    private void CrouchStart()
    {
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Clamp(transform.localScale.y - crouchScaleSpeed, crouchHeight, crouchHeight), transform.localScale.z);
    }


    // Subroutine for stopcrouching
    private void CrouchStop()
    {
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Clamp(transform.localScale.y + crouchScaleSpeed, crouchHeight, originalScale), transform.localScale.z);
    }
    #endregion


    #region Camera
    // Subroutine for the camera based on input by the player
    private void CameraRotation()
    {
        float mouseX = UnityEngine.Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
        float mouseY = UnityEngine.Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;

        Vector3 rotation = playerCamera.transform.localRotation.eulerAngles;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation * flip, yRotation * flip, playerCenter.transform.localEulerAngles.z);
        orientation.transform.localRotation = Quaternion.Euler(0, yRotation * flip, 0);
    }


    // The camera flips based on the angles of the player center
    public void CameraFlip()
    {
        if (Mathf.Abs(playerCenter.transform.localEulerAngles.z) == 180) flip = -1;
        else flip = 1;
    }


    // Getter for the current Z axis
    public float getCurrentZAxis()
    {
        return playerCenter.transform.localEulerAngles.z;
    }


    // Setting the new Z Axis
    public void flipZAxis(Quaternion rotation)
    {
        playerCenter.transform.rotation = rotation;
    }
    #endregion


    #region Sphere/Raycasting
    // Subroutine which returns a bool on weather or not its touching floor
    private bool sphereCasting()
    {
        sphereCastOrigin = (transform.position + (Vector3.up - (Vector3.up * transform.localScale.y)) * flip);
        sphereCastDirection = -transform.up * flip;
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


    // Subroutine for raycasting to a component of ISwitchable
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
                    component.Switch(new Vector3(currentTempLocation.x, currentTempLocation.y - transform.localScale.y + (component.yScale * 3 / 2), currentTempLocation.z));
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


    // Subroutine for raycasting to a component of IPressable
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
    // Subroutine which returns a bool if the player drops below a threshold
    private bool CheckDead()
    {
        if (healtBar?.getHealth() <= 0)
        {
            return true;
        }
        return false;
    }


    // Subroutine for falldamage calucaltion and wheather the it should take damage or not
    public void fallDamage(float fallVelocity)
    {
        if (Mathf.Abs(fallVelocity) > minFallVelocityToGainDamage)
        {
            int damageTaken = (int)Mathf.Abs((fallVelocity - minFallVelocityToGainDamage) * fallDamageMultiplier);
            takeDamage(damageTaken);
        }
    }


    // Subroutine by IDamagable, player takes damage based on given amount
    public void takeDamage(int amount)
    {
        healtBar.setHealth(healtBar.getHealth() - amount);
    }


    // Subroutine if dead, then start a counter to reset the scene
    public void Dead()
    {
        healtBar.setHealth(0);
        StartCoroutine(DeadCounter());
    }

    
    // IEnumerator to reset the scene
    private IEnumerator DeadCounter()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion


    #region Collision
    private void OnTriggerStay(Collider collider)
    {
        // If staying in an object of Interface IPressable then press this object (most likly a floor button)
        IPressable component = collider.GetComponent<IPressable>();
        if (component != null) component.PressObject();

    }


    private void OnTriggerExit(Collider collider)
    {
        // If exiting this object then unpress it
        IPressable component = collider.GetComponent<IPressable>();
        if (component != null) component.UnpressObject();
    }


    private void OnCollisionStay(Collision collision)
    {
        // Part of a two step ground detection system together with SphereCasting()
        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 8)
        {
            collisionOnGround = true;
        }
    }
    #endregion
}
