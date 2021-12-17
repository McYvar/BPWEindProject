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
        
        // Initialise the healbar
        healtBar.SetMaxHealth(100);

        flip = 1;
    }


    public void Update()
    {
        CameraFlip();

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
    private void Input()
    {
        if (UnityEngine.Input.GetKey(KeyCode.LeftControl)) CrouchStart();
        else CrouchStop();

        if (UnityEngine.Input.GetMouseButtonDown(0)) onImpact();
        if (UnityEngine.Input.GetKeyDown(KeyCode.E)) OnPress();
    }
    #endregion


    #region Crouching
    private void CrouchStart()
    {
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Clamp(transform.localScale.y - crouchScaleSpeed, crouchHeight, crouchHeight), transform.localScale.z);
    }


    private void CrouchStop()
    {
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Clamp(transform.localScale.y + crouchScaleSpeed, crouchHeight, originalScale), transform.localScale.z);
    }
    #endregion


    #region Camera
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

    public void CameraFlip()
    {
        if (Mathf.Abs(playerCenter.transform.localEulerAngles.z) == 180) flip = -1;
        else flip = 1;
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


    #region Sphere/Raycasting
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(sphereCastOrigin + sphereCastDirection * sphereCastHitDistance, sphereCastRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * 1000);
    }
    #endregion


    #region Damage
    private bool CheckDead()
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
            int damageTaken = (int)Mathf.Abs((fallVelocity - minFallVelocityToGainDamage) * fallDamageMultiplier);
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


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 8)
        {
            collisionOnGround = true;
        }
    }
    #endregion
}
