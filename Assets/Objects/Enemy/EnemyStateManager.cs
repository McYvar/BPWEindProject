using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour, IDamagable, ISwitchable, IPressable
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
    private float timedAttack;
    private bool isDead;
    public bool activateObject { get; set; }
    public bool stayActive { get; set; }


    // Switching with enemy
    public Vector3 location { get; set; }
    public float yScale { get; set; }

    public bool switching;

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
        isDead = false;

        yScale = transform.localScale.y;
        switching = false;
    }


    private void Update()
    {
        isGrounded = sphereCasting();

        // Actually outdated finite state machine in compare to player, less flexible, but works fine
        currentState.UpdateState(this);

        // Check if the gravity faces down or up (doesn't work well with the navmesh)
        if (Physics.gravity.y > 0 && up)
        {
            up = false;
            SwitchState(airborneState);
        }

        if (Physics.gravity.y < 0 && !up)
        {
            up = true;
            SwitchState(airborneState);
        }

        // If the enemy dies it will be switched to the deadstate
        if (CheckDead() && !isDead)
        {
            isDead = true;
            SwitchState(deadState);
        }

        // Save the current location each frame because you can teleport with the enemy
        location = new Vector3(transform.position.x, transform.position.y - transform.localScale.y, transform.position.z);


        if (switching && timer <= 0)
        {
            switching = false;
        }
        else timer -= Time.deltaTime;
    }


    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }
    #endregion


    #region Player detection system (including raycast)
    public bool PlayerDetectionCheck()
    {
        // If the player comes in a certain radius then it returns a true value
        bool playerInDetectionRadius = Physics.CheckSphere(transform.position, detectionRadius, whatIsPlayer);
        if (playerInDetectionRadius && PlayerInLineOfSeight()) return true;
        return false;
    }


    public bool PlayerAttackingCheck()
    {
        // If the player comes in a certain radius then it returns a true value
        bool playerInAttackRadius = Physics.CheckSphere(transform.position, attackRadius, whatIsPlayer);
        if (playerInAttackRadius && PlayerInLineOfSeight()) return true;
        return false;
    }


    private bool PlayerInLineOfSeight()
    {
        // Can only detects player when its not hiding behind an object, will still walk towards last seen point
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
        // If the player is in radius the enemy starts chacing the player
        if (switching) return;
        enemy.SetDestination(player.transform.position);
    }


    public void AttackPlayer()
    {
        // If the player is in radius the enemy starts attacking the player
        if (switching) return;
        enemy.SetDestination(transform.position);

        // Creepy but he keeps looking at the player
        transform.LookAt(player.transform.position, Vector3.up);

        // Attacks based on a simple timer
        if (timedAttack <= 0)
        {
            timedAttack = timeTillNextAttack;
            GameObject newThrowable = Instantiate(throwable, transform.position + (transform.forward * transform.localScale.z), Quaternion.identity);
            Rigidbody throwableRb = newThrowable.GetComponent<Rigidbody>();
            throwableRb.AddForce(transform.forward * 50, ForceMode.VelocityChange);
        }

        timedAttack -= Time.deltaTime;
    }


    private void OnDestroy()
    {
        // When the enemy dies a mechanism can be triggered like a door or something
        PressObject();
    }
    #endregion


    #region Spherecasting and ground detection
    // Subroutine that returns a bool based on weather the enemy is close to ground or not
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


    // Subroutine to kind of enable and disable the rigidbody of this enemy
    public void EnableConstrains()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }


    // Subroutine to renable that
    public void DisableConstrains()
    {
        rb.constraints = RigidbodyConstraints.None;
        if (!isDead)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }
    #endregion


    #region Damage
    // Subroutine to detect weather the enemy falls at a high velocity and calculates if it should take damage and how much damage
    public void fallDamage(float fallVelocity)
    {
        if (Mathf.Abs(fallVelocity) > minFallVelocityToGainDamage)
        {
            int damageTaken = (int)Mathf.Abs((fallVelocity - minFallVelocityToGainDamage) * fallDamageMultiplier);
            takeDamage(damageTaken);
            Debug.Log(damageTaken);
        }
    }


    // Subroutine for Interface IDamagable, takes damages based on amount
    public void takeDamage(int amount)
    {
        healt -= amount;
    }


    // Subroutine for the enmies health
    private void setHealth(int amount)
    {
        healt = amount;
    }


    // Subroutine to check if the enemy is dead or not
    public bool CheckDead()
    {
        if (healt <= 0)
        {
            return true;
        }
        return false;
    }


    // Subroutine to make the enemy smaller up on dying and then starts a deadcounter
    public void Dead()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
        StartCoroutine(DeadCounter());
    }

    
    // IENumerator for deadcounter
    private IEnumerator DeadCounter()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
    #endregion


    #region FSM related stuff
    // Older but working version of the finite state machine for the enemy
    public void SwitchState(EnemyBaseState nextState)
    {
        currentState?.ExitState(this);
        currentState = nextState;
        currentState?.EnterState(this);
    }


    // Subroutine for state machine, if the enemy is in a certain state but has to switch to another one but thereafter has to come back to this specific state, then this routine remembers that state
    // and then after running trough the new state, you can switch back to this state
    public void SwitchAndRememberLastState(EnemyBaseState nextState, EnemyBaseState previousState)
    {
        this.previousState = previousState;
        SwitchState(nextState);
    }
    #endregion


    #region Switiching with player
    // Subroutine for player switching with the enemy, from Interface ISwitchable
    public void Switch(Vector3 location)
    {
        StartCoroutine(switchCooldown(location));
    }


    // IEnumerator for a cooldown on switching, you can switch again, but the enemy has to go trough a certain steps before being able to move again
    private IEnumerator switchCooldown(Vector3 location)
    {
        timer = 1;
        switching = true;
        yield return new WaitForFixedUpdate();
        location.y += player.transform.localScale.y;
        transform.position = location;
    }
    #endregion


    #region IPressable Interface stuff
    // Subroutine by IPressable, when the enemy dies a mechanism could be triggered
    public void PressObject()
    {
        activateObject = true;
    }

    public void UnpressObject()
    {
    }
    #endregion
}
