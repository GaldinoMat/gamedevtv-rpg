using System;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{

    public class AiController : MonoBehaviour
    {
        Vector3 guardPosition;

        GameObject playerObj;

        Fighter fighter;

        Health health;

        Mover mover;

        [SerializeField] PatrolPath path;

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float dwellingTime = 0f;
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = .2f;

        float timeSinceAlert = Mathf.Infinity;
        float timeSinceLastDwell = Mathf.Infinity;

        int currentWaypointIndex = 0;

        private void Start()
        {
            guardPosition = transform.position;

            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            playerObj = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (CanChase())
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

        private void UpdateTimers()
        {
            timeSinceAlert += Time.deltaTime;
            timeSinceLastDwell += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceAlert = 0;
            fighter.Attack(playerObj);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

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

        private bool CanChase()
        {
            return Vector3.Distance(transform.position, playerObj.transform.position) <= chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
