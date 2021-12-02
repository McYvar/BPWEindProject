using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    private EnemyBaseState currentState;
    public EnemyBaseState previousState;
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyChaseState chaceState = new EnemyChaseState();
    public EnemyAttackState attackState = new EnemyAttackState();
    public EnemyAirborneState airborneState = new EnemyAirborneState();

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

    // Enemy placement (rigidbody based)
    public bool grounded;
    private Rigidbody rb;


    private void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        currentState = idleState;
        currentState?.EnterState(this);

        flip = 1;
    }


    private void Update()
    {
        grounded = sphereCasting();

        currentState.UpdateState(this);
    }


    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }


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
        Debug.DrawLine(transform.position, player.transform.position);
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
    }


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
        Debug.DrawLine(sphereCastOrigin, sphereCastOrigin * sphereCastHitDistance);
        Gizmos.DrawSphere(sphereCastOrigin + sphereCastDirection * sphereCastHitDistance, sphereCastRadius);
    }

}
