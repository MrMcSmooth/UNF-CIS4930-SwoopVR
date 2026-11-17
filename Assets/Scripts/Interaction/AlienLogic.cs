using UnityEngine;
using UnityEngine.AI;

public class AlienLogic : MonoBehaviour
{
    [Header("Alien Scare")]
    [SerializeField] private Material newMaterial;
    [SerializeField] private float sMaxDuration = 100f;
    [SerializeField] public float sDuration = 100f;
    [SerializeField] private float sDrain = 5f;

    [Header("Movement")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform patrolPointA;
    [SerializeField] private Transform patrolPointB;
    [SerializeField] private Transform hideSpot;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float scaredSpeed = 5f;
    [SerializeField] private float pointDistance = 0.5f;

    [Header("Scared Movement")]
    [SerializeField] private float randomOffset = 2f;
    [SerializeField] private float directionChangeInterval = 0.5f;

    [Header("Hiding")]
    [SerializeField] private float hideDuration = 5f;

    private float hideTimer;
    private Material originalMaterial;
    private Renderer alienRenderer;

    private bool isScared = false;
    private bool isHiding = false;

    private Transform currentPatrolPoint;
    private float directionChangeTimer;

    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        alienRenderer = GetComponentInChildren<Renderer>();

        if (alienRenderer != null)
        {
            originalMaterial = alienRenderer.material;
        }

        currentPatrolPoint = patrolPointA;

        if (agent != null && currentPatrolPoint != null)
        {
            agent.speed = walkSpeed;
            agent.SetDestination(currentPatrolPoint.position);
        }
    }

    void Update()
    {
        if(isHiding)
        {
            hideTimer -= Time.deltaTime;

            if (hideTimer <= 0f)
            {
                StopHiding();
            }

            return;
        }

        if (isScared)
        {
            DrainAlien(sDrain * Time.deltaTime);

            ScaredMovement();


            //Debug.Log(sDuration);
            if (sDuration <= 0)
            {
                isScared = false;
                agent.speed = walkSpeed;

                currentPatrolPoint = patrolPointA;
                agent.SetDestination(currentPatrolPoint.position);
            }
        }
        else
        {
            Patrol();
        }
    }

    public void Patrol()
    {
        if(agent == null || patrolPointA == null || patrolPointB == null)
        {
            return;
        }
        
        agent.speed = walkSpeed;

        if (!agent.pathPending && agent.remainingDistance <= pointDistance)
        {
            if(currentPatrolPoint == patrolPointA)
            {
                currentPatrolPoint = patrolPointB;
            }
            else
            {
                currentPatrolPoint = patrolPointA;
            }

            agent.SetDestination(currentPatrolPoint.position);
        }
    }

    private void ScaredMovement()
    {
        Debug.Log("ScaredMovement running");

        if (agent == null)
        {
            Debug.LogError("NO NAVMESH AGENT!");
            return;
        }

        if (hideSpot == null)
        {
            Debug.LogError("NO HIDE SPOT!");
            return;
        }
        if (agent == null || hideSpot == null)
        {
            return;
        }

        agent.speed = scaredSpeed;
        
        float distanceToHideSpot = Vector3.Distance(transform.position, hideSpot.position);
        
        if(distanceToHideSpot <= 2f)
        {
            agent.SetDestination(hideSpot.position);

            if(distanceToHideSpot <= pointDistance)
            {
                Hide();
            }

            return;
        }

        directionChangeTimer -= Time.deltaTime;

        if(directionChangeTimer <= 0f)
        {
            Vector3 directionToHideSpot = (hideSpot.position - transform.position).normalized;

            Vector3 randomDirection = Random.insideUnitSphere * randomOffset;

            randomDirection.y = 0f;

            Vector3 nextPosition = transform.position + directionToHideSpot * 3f + randomDirection;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(nextPosition, out hit, randomOffset, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            directionChangeTimer = directionChangeInterval;
        }
    }


    public void Scared()
    {
        if (isScared || isHiding)
        {
            return;
        }

        if (alienRenderer != null && newMaterial != null)
        {
            alienRenderer.material = newMaterial;
        }

        isScared = true;

        sDuration = sMaxDuration;

        directionChangeTimer = 0f;

        if (agent != null)
        {
            agent.speed = scaredSpeed;
        }
    }

    private void Hide()
    {
        isHiding = true;
        isScared = false;

        agent.ResetPath();

        hideTimer = hideDuration;

        Debug.Log("Alien is hiding!");
    }

    private void StopHiding()
    {
        isHiding = false;

        if (alienRenderer != null && originalMaterial != null)
        {
            alienRenderer.material = originalMaterial;
        }

        agent.speed = walkSpeed;

        currentPatrolPoint = patrolPointA;

        if (currentPatrolPoint != null)
        {
            agent.SetDestination(currentPatrolPoint.position);
        }

        Debug.Log("Alien stopped hiding!");
    }

    public void DrainAlien(float amount)
    {
        sDuration -= amount;
        sDuration = Mathf.Clamp(sDuration, 0f, sMaxDuration);
    }
}
