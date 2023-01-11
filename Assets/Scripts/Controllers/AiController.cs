using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{

    public class AiController : MonoBehaviour
    {
        Vector3 guardPosition;

        Fighter fighter;

        Health health;

        Mover mover;

        GameObject playerObj;

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        float timeSinceAlert = Mathf.Infinity;

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
                GuardBehaviour();
            }

            timeSinceAlert += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceAlert = 0;
            fighter.attack(playerObj);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void GuardBehaviour()
        {
            fighter.Cancel();
            mover.startMoveAction(guardPosition);
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
