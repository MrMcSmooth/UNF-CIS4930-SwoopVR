
using UnityEngine;
using UnityEngine.AI;
using MimicSpace;

[RequireComponent(typeof(Mimic))]
public class AlienLogic : MonoBehaviour
{
    [SerializeField] public float sDuration = 100f;
    private enum AlienState
    {
        Patrol,
        Chase,
        Flee,
        Hide
    }

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Transform patrolPointA;
    [SerializeField] private Transform patrolPointB;
    [SerializeField] private Transform hideSpot;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float scaredSpeed = 5f;
    [SerializeField] private float pointDistance = 0.5f;

    [Header("Chase Settings")]
    [SerializeField] private float chaseDistance = 8f;
    [SerializeField] private float loseDistance = 12f;
    [SerializeField] private float chaseStopDistance = 1.5f;
    [SerializeField] private float chaseUpdateInterval = 0.2f;

    [Header("Mimic Animation")]
    [SerializeField] private float bodyHeight = 0.8f;
    [SerializeField] private float rotationSpeed = 8f;

    [Header("Alien Scare")]
    [SerializeField] private Material newMaterial;
    [SerializeField] private float sMaxDuration = 12f;
    [SerializeField] private float sDrain = 1f;
    public float sDuration = 12f;

    [Header("Hiding")]
    [SerializeField] private float hideDuration = 5f;

    [Header("Debug")]
    [SerializeField] private AlienState currentState = AlienState.Patrol;

    private Mimic myMimic;
    private Renderer alienRenderer;
    private Material originalMaterial;
    private Transform currentPatrolPoint;

    private float hideTimer;
    private float nextChaseUpdate;

    void Start()
    {
        myMimic = GetComponent<Mimic>();

        if (player == null && Camera.main != null)
            player = Camera.main.transform;

        if (agent == null || player == null ||
            myMimic == null || !agent.isOnNavMesh)
        {
            Debug.LogError(
                "AlienLogic: Missing reference or NavMeshAgent is not on a NavMesh.",
                this
            );

            enabled = false;
            return;
        }

        // The navigator must be a separate object
        // because the Mimic body floats above the ground.
        if (agent.gameObject == gameObject)
        {
            Debug.LogError(
                "AlienLogic: Assign a separate MimicNavigator object.",
                this
            );

            enabled = false;
            return;
        }

        agent.updateRotation = false;

        alienRenderer = GetComponent<Renderer>();

        if (alienRenderer == null)
            alienRenderer = GetComponentInChildren<Renderer>();

        if (alienRenderer != null)
            originalMaterial = alienRenderer.material;

        sDuration = sMaxDuration;
        currentPatrolPoint = patrolPointA;

        BeginPatrol();
    }

    void Update()
    {
        if (agent == null || !agent.isOnNavMesh)
            return;

        if (currentState == AlienState.Patrol ||
            currentState == AlienState.Chase)
        {
            float distance = HorizontalDistance(
                agent.transform.position,
                player.position
            );

            if (currentState == AlienState.Patrol &&
                distance <= chaseDistance)
            {
                BeginChase();
            }
            else if (currentState == AlienState.Chase &&
                     distance >= loseDistance)
            {
                BeginPatrol();
            }
        }

        switch (currentState)
        {
            case AlienState.Patrol:
                Patrol();
                break;

            case AlienState.Chase:
                Chase();
                break;

            case AlienState.Flee:
                Flee();
                break;

            case AlienState.Hide:
                UpdateHiding();
                break;
        }

        UpdateMimicMovement();
    }



    private void BeginPatrol()
    {
        currentState = AlienState.Patrol;

        agent.isStopped = false;
        agent.speed = walkSpeed;
        agent.stoppingDistance = 0.1f;

        if (currentPatrolPoint == null)
            currentPatrolPoint = patrolPointA != null
                ? patrolPointA : patrolPointB;

        if (currentPatrolPoint != null)
            SetNavDestination(currentPatrolPoint.position);
        else
            agent.ResetPath();
    }

    private void Patrol()
    {
        if (patrolPointA == null || patrolPointB == null)
            return;

        if (!agent.pathPending &&
            agent.hasPath &&
            agent.remainingDistance <= pointDistance)
        {
            currentPatrolPoint =
                currentPatrolPoint == patrolPointA
                ? patrolPointB
                : patrolPointA;

            SetNavDestination(currentPatrolPoint.position);
        }
    }

    private void BeginChase()
    {
        currentState = AlienState.Chase;

        agent.isStopped = false;
        agent.speed = chaseSpeed;
        agent.stoppingDistance = chaseStopDistance;

        nextChaseUpdate = 0f;
    }

    private void Chase()
    {
        if (Time.time < nextChaseUpdate)
            return;

        SetNavDestination(player.position);

        nextChaseUpdate =
            Time.time + chaseUpdateInterval;
    }

    public void Scared()
    {
        if (!isActiveAndEnabled ||
            agent == null ||
            !agent.isOnNavMesh ||
            currentState == AlienState.Flee ||
            currentState == AlienState.Hide)
        {
            return;
        }

        if (hideSpot == null)
        {
            Debug.LogWarning("AlienLogic: No hide spot assigned.");
            return;
        }

        currentState = AlienState.Flee;

        sDuration = sMaxDuration;

        if (alienRenderer != null && newMaterial != null)
            alienRenderer.material = newMaterial;

        agent.isStopped = false;
        agent.speed = scaredSpeed;
        agent.stoppingDistance = 0.1f;

        SetNavDestination(hideSpot.position);

        Debug.Log("Mimic is fleeing!");
    }

    private void Flee()
    {
        DrainAlien(sDrain * Time.deltaTime);

        if (sDuration <= 0f)
        {
            FinishScare();
            return;
        }
        if (!agent.pathPending &&
            agent.hasPath &&
            agent.remainingDistance <= pointDistance)
        {
            BeginHiding();
        }
    }


    private void BeginHiding()
    {
        currentState = AlienState.Hide;

        agent.isStopped = true;
        agent.ResetPath();

        hideTimer = hideDuration;

        Debug.Log("Mimic is hiding!");
    }

    private void UpdateHiding()
    {
        hideTimer -= Time.deltaTime;

        if (hideTimer <= 0f)
            FinishScare();
    }

    private void FinishScare()
    {
        if (alienRenderer != null &&
            originalMaterial != null)
        {
            alienRenderer.material = originalMaterial;
        }

        sDuration = sMaxDuration;

        float distance = HorizontalDistance(
            agent.transform.position,
            player.position
        );

        if (distance <= chaseDistance)
            BeginChase();
        else
            BeginPatrol();

        Debug.Log("Mimic finished hiding/fleeing!");
    }

    public void DrainAlien(float amount)
    {
        sDuration -= amount;
        sDuration = Mathf.Clamp(
            sDuration, 0f, sMaxDuration
        );
    }

    private void SetNavDestination(Vector3 destination)
    {
        if (NavMesh.SamplePosition(
            destination,
            out NavMeshHit hit,
            3f,
            agent.areaMask))
        {
            agent.SetDestination(hit.position);
        }
    }

    private float HorizontalDistance(Vector3 a, Vector3 b)
    {
        a.y = 0f;
        b.y = 0f;
        return Vector3.Distance(a, b);
    }

    private void UpdateMimicMovement()
    {
        transform.position =
            agent.transform.position +
            Vector3.up * bodyHeight;

        Vector3 velocity = agent.velocity;
        velocity.y = 0f;

        myMimic.velocity = velocity;

        if (velocity.sqrMagnitude > 0.04f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(
                    velocity,
                    Vector3.up
                );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
