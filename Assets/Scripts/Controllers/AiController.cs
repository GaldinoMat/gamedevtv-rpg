using System;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{

    public class AiController : MonoBehaviour
    {
        LazyValue<Vector3> guardPosition;

        GameObject playerObj;

        Fighter fighter;

        Health health;

        Mover mover;

        [SerializeField] PatrolPath path;

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float aggroCooldown = 5f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float dwellingTime = 0f;
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = .2f;
        [SerializeField] float shoutDistance = 10f;


        float timeSinceAlert = Mathf.Infinity;
        float timeSinceLastDwell = Mathf.Infinity;
        float timeSinceAggravated = Mathf.Infinity;

        int currentWaypointIndex = 0;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            playerObj = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>();

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (IsAggravated() && fighter.CanAttack(playerObj))
            {
                AttackBehaviour();
            }
            else if (timeSinceAlert <= suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        public void Aggravate()
        {
            timeSinceAggravated = 0;
        }

        private void UpdateTimers()
        {
            timeSinceAlert += Time.deltaTime;
            timeSinceLastDwell += Time.deltaTime;
            timeSinceAggravated += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceAlert = 0;
            fighter.Attack(playerObj);

            AggravateNearbyEnemies();
        }

        private void AggravateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);

            foreach (RaycastHit enemy in hits)
            {
                AiController foundEnemy = enemy.transform.GetComponent<AiController>();

                if (!foundEnemy) continue;

                foundEnemy.Aggravate();
            }
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (path != null)
            {
                if (AtWaypoint())
                {
                    timeSinceLastDwell = 0;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceLastDwell >= dwellingTime)
            {
                fighter.Cancel();
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return path.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = path.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());

            return distanceToWaypoint < waypointTolerance;
        }

        private bool IsAggravated()
        {
            return Vector3.Distance(transform.position, playerObj.transform.position) <= chaseDistance || timeSinceAggravated < aggroCooldown;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
