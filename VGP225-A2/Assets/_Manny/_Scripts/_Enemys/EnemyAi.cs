using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Renderer visible;
    [SerializeField] private Animator anim;
    [SerializeField] private LookAtPlayer look;

    [Header("Settings")]
    [SerializeField] private bool disappearWhenSpotted;
    [SerializeField] private bool willAlwaysFlee;

    [Header("Patrolling")]
    [SerializeField] private float walkPointRange;
    private Vector3 walkPoint;
    private bool walkPointSet;

    [Header("Combat")]
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float timeBetweenAttacks = 1f;
    [SerializeField] private float shootDelay = 1f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject sword;
    [SerializeField] private bool isMelee;
    [SerializeField] private bool isRanged;
    private bool alreadyAttacked;

    [Header("Fleeing")]
    [SerializeField] private float fleeDistance = 10f;
    [SerializeField] private float fleeHealthThreshold = 20f;
    [SerializeField] private float safeDistance = 15f; // NEW
    private bool isFleeing = false;

    [Header("Health")]
    [SerializeField] private int health = 100;

    [Header("Pickup On Death")]
    [SerializeField] private GameObject pickupPrefab;

    // Runtime States
    [HideInInspector] public bool playerInSightRange;
    [HideInInspector] public bool playerInAttackRange;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        look = GetComponent<LookAtPlayer>();
        visible = GetComponentInChildren<Renderer>();
        anim = GetComponentInChildren<Animator>();

        isMelee = (projectile == null);
        isRanged = (sword == null);
    }

    private void Update()
    {
        if (player == null) return;

        bool seen = look.Spotted;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Flee if low on health
        if (health <= fleeHealthThreshold || (willAlwaysFlee && playerInSightRange))
        {
            if (distanceToPlayer < safeDistance)
            {
                Flee();
            }
            else if (isFleeing)
            {
                // Stop fleeing if already far enough
                isFleeing = false;
                anim.SetBool("isWalking", false);
                agent.ResetPath();
            }
            return;
        }

        if (!seen || !disappearWhenSpotted)
        {
            HandleState();
        }
        else
        {
            Invoke(nameof(Spotted), 2f);
        }
    }

    private void HandleState()
    {
        visible.enabled = true;

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            if (willAlwaysFlee)
            {
                Flee();
            }
            else
            {
                ChasePlayer();
            }
        }
        else if (playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void Spotted()
    {
        agent.SetDestination(transform.position);
        visible.enabled = false;
        Debug.Log("AHHHHH");
    }

    private void Patroling()
    {
        anim.SetBool("isWalking", true);

        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            anim.SetBool("isWalking", false);
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        anim.SetBool("isWalking", true);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        anim.SetBool("isWalking", false);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            anim.SetTrigger("Attack");

            if (isRanged)
            {
                Invoke(nameof(ShootProjectile), shootDelay);
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ShootProjectile()
    {
        Rigidbody rb = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        rb.AddForce(transform.up * 1f, ForceMode.Impulse);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void Flee()
    {
        if (!isFleeing)
        {
            isFleeing = true;
            anim.SetBool("isWalking", true);
        }

        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeTarget, out hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log("Fleeing to: " + hit.position);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        anim.SetTrigger("Hit");

        if (health <= 0)
        {
            anim.SetTrigger("Death");
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        agent.isStopped = true;
        GetComponent<Collider>().enabled = false;
        this.enabled = false; // Disable this script
        Invoke(nameof(DestroyAfterAnimation), 2f);
    }

    private void DestroyAfterAnimation()
    {
        if (pickupPrefab != null)
        {
            Instantiate(pickupPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Debug.Log("Enemy hit by weapon!");
            TakeDamage(999); // Or appropriate damage value
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
