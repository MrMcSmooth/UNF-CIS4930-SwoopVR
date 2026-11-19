
using UnityEngine;
using UnityEngine.AI;

namespace MimicSpace
{
    [RequireComponent(typeof(Mimic))]
    public class MimicChase : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform player;
        [SerializeField] private NavMeshAgent navigator;

        [Header("Movement")]
        [SerializeField] private float bodyHeight = 0.8f;
        [SerializeField] private float turnSpeed = 8f;
        [SerializeField] private float updateInterval = 0.2f;

        private Mimic myMimic;
        private float nextUpdate;

        void Start()
        {
            myMimic = GetComponent<Mimic>();

            if (player == null && Camera.main != null)
                player = Camera.main.transform;

            if (player == null || navigator == null)
            {
                Debug.LogError("MimicChase: Missing player or navigator.");
                enabled = false;
                return;
            }

            // Place the invisible navigator on the NavMesh.
            Vector3 groundPosition =
                transform.position - Vector3.up * bodyHeight;

            if (NavMesh.SamplePosition(
                groundPosition,
                out NavMeshHit start,
                3f,
                navigator.areaMask))
            {
                if (!navigator.Warp(start.position))
                {
                    Debug.LogError("Navigator could not reach the NavMesh.");
                    enabled = false;
                    return;
                }
            }
            else
            {
                Debug.LogError("No NavMesh found near the Mimic.");
                enabled = false;
                return;
            }

            navigator.updateRotation = false;
            myMimic.velocity = Vector3.zero;
        }

        void Update()
        {
            if (!navigator.isOnNavMesh)
                return;

            // Update the destination periodically.
            if (Time.time >= nextUpdate)
            {
                if (NavMesh.SamplePosition(
                    player.position,
                    out NavMeshHit target,
                    3f,
                    navigator.areaMask))
                {
                    navigator.SetDestination(target.position);
                }

                nextUpdate = Time.time + updateInterval;
            }

            // Follow the navigator while keeping the body raised.
            transform.position =
                navigator.transform.position + Vector3.up * bodyHeight;

            // Update the procedural leg movement.
            Vector3 movement = navigator.velocity;
            movement.y = 0f;

            myMimic.velocity = movement;

            // Turn to face the movement direction.
            if (movement.sqrMagnitude > 0.04f)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(movement, Vector3.up);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    turnSpeed * Time.deltaTime
                );
            }
        }

        void OnDisable()
        {
            if (myMimic != null)
                myMimic.velocity = Vector3.zero;

            if (navigator != null &&
                navigator.enabled &&
                navigator.isOnNavMesh)
            {
                navigator.ResetPath();
            }
        }
    }
}
