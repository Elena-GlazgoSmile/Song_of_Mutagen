using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 1f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Movement Settings")]
    [SerializeField] private float stoppingDistance = 1.5f;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1f;

    private NavMeshAgent agent;
    public Transform player;
    private bool isPlayerDetected = false;
    private float nextAttackTime = 0f;
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on " + gameObject.name);
        }

        agent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        DetectPlayer();
        if (isPlayerDetected && player != null)
        {
            MoveToPlayer();
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    private void DetectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        if (hitColliders.Length > 0)
        {
            if (!isPlayerDetected)
            {
                Debug.Log("Player detected!");
            }

            isPlayerDetected = true;
            player = hitColliders[0].transform;
        }
        
        //если враг обнаружил игрока, то постоянно его преследует
        //else
        //{
        //    isPlayerDetected = false;
        //    player = null;

        //    if (agent.isOnNavMesh && agent.remainingDistance > 0.1f)
        //    {
        //        agent.ResetPath();
        //    }
        //}
    }

    private void MoveToPlayer()
    {
        //анимация ходьбы
        animator.SetBool("IsWalking", true);

        if (agent.isOnNavMesh && player != null)
        {
            agent.SetDestination(player.position);

            // Проверяем, достиг ли враг игрока
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= stoppingDistance)
            {
                TryAttackPlayer();
            }
        }
    }

    private void TryAttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            Debug.Log("Enemy killed the player!");
        }
    }


    // Визуализация радиуса обнаружения в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}