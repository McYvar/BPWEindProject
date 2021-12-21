using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour, IDamagable
{
    #region Variables and such
    private EnemyBaseState currentState;
    public EnemyBaseState previousState;
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyChaseState chaceState = new EnemyChaseState();
    public EnemyAttackState attackState = new EnemyAttackState();
    public EnemyAirborneState airborneState = new EnemyAirborneState();
    public EnemyDeadState deadState = new EnemyDeadState();

    // Player detection
    public float detectionRadius, attackRadius;
    public LayerMask whatIsPlayer, whatIsGround;
    public GameObject player;
    public NavMeshAgent enemy;

    // Variables for spherecast
    public GameObject sphereCastHitObject;
    public float sphereCastRadius;
    public float sphereCastMaxDistance;
    public LayerMask sphereCastLayerMask;

    private float sphereCastHitDistance;
    private Vector3 sphereCastOrigin;
    private Vector3 sphereCastDirection;

    // Enemy orientation
    public float flip;
    public GameObject enemyCenter;

    // Enemy placement (rigidbody based)
    public bool isGrounded;
    public Rigidbody rb;
    private bool up;

    // Enemy damage and health
    public int healt { get; set; }
    public int startingHealth;
    public float minFallVelocityToGainDamage;
    public float fallDamageMultiplier;

    public GameObject throwable;
    public float timeTillNextAttack;
    private float timer;
    #endregion


    #region Awake/Start/Update/Fixedupdate
    private void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        setHealth(startingHealth);
    }


    private void Start()
    {
        currentState = idleState;
        currentState?.EnterState(this);

        flip = 1;
        if (Physics.gravity.y < 0) up = true;
        else up = false;
    }


    private void Update()
    {
        isGrounded = sphereCasting();

        currentState.UpdateState(this);

        if (Physics.gravity.y > 0 && up)
        {
            up = false;
            SwitchState(airborneState);
            Debug.Log("fly up");
        }

        if (Physics.gravity.y < 0 && !up)
        {
            up = true;
            SwitchState(airborneState);
            Debug.Log("fly down");
        }
    }


    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }
    #endregion


    #region Player detection system (including raycast)
    public bool PlayerDetectionCheck()
    {
        bool playerInDetectionRadius = Physics.CheckSphere(transform.position, detectionRadius, whatIsPlayer);
        if (playerInDetectionRadius && PlayerInLineOfSeight()) return true;
        return false;
    }


    public bool PlayerAttackingCheck()
    { 
        bool playerInAttackRadius = Physics.CheckSphere(transform.position, attackRadius, whatIsPlayer);
        if (playerInAttackRadius && PlayerInLineOfSeight()) return true;
        return false;
    }


    private bool PlayerInLineOfSeight()
    {
        bool playerInLineOfSeight = true;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit))
        {
            if (hit.collider.CompareTag("Player")) playerInLineOfSeight = true;
            else playerInLineOfSeight = false;
        }

        if (playerInLineOfSeight) return true;
        return false;
    }


    public void ChacePlayer()
    {
        enemy.SetDestination(player.transform.position);
    }


    public void AttackPlayer()
    {
        enemy.SetDestination(transform.position);

        transform.LookAt(player.transform.position, Vector3.up);

        if (timer <= 0)
        {
            timer = timeTillNextAttack;
            GameObject newThrowable = Instantiate(throwable, transform.position + (transform.forward * transform.localScale.z), Quaternion.identity);
            Rigidbody throwableRb = newThrowable.GetComponent<Rigidbody>();
            throwableRb.AddForce(transform.forward * 50, ForceMode.VelocityChange);
        }

        timer -= Time.deltaTime;
    }
    #endregion


    #region Spherecasting and ground detection
    public bool sphereCasting()
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

    public void EnableConstrains()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }


    public void DisableConstrains()
    {
        rb.constraints = RigidbodyConstraints.None;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(sphereCastOrigin + sphereCastDirection * sphereCastHitDistance, sphereCastRadius);
    }
    #endregion


    #region Damage

    public void fallDamage(float fallVelocity)
    {
        if (Mathf.Abs(fallVelocity) > minFallVelocityToGainDamage)
        {
            int damageTaken = (int)Mathf.Abs((fallVelocity - minFallVelocityToGainDamage) * fallDamageMultiplier);
            takeDamage(damageTaken);
            Debug.Log(damageTaken);
        }
    }


    public void takeDamage(int amount)
    {
        healt -= amount;
        Debug.Log(amount);
    }


    private void setHealth(int amount)
    {
        healt = amount;
    }


    public bool CheckDead()
    {
        if (healt <= 0)
        {
            return true;
        }
        return false;
    }


    public void Dead()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
        StartCoroutine(DeadCounter());
    }


    private IEnumerator DeadCounter()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
    #endregion


    public void SwitchState(EnemyBaseState nextState)
    {
        currentState?.ExitState(this);
        currentState = nextState;
        currentState?.EnterState(this);
    }


    public void SwitchAndRememberLastState(EnemyBaseState nextState, EnemyBaseState previousState)
    {
        this.previousState = previousState;
        SwitchState(nextState);
    }

}
