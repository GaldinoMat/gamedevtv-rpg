using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField]
        Transform target;

        NavMeshAgent agent;

        Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            updateAnimator();
        }

        private void updateAnimator()
        {
            Vector3 velocity = agent.velocity;
            float localZ = transform.InverseTransformDirection(velocity).z;

            anim.SetFloat("forwardSpeed", localZ);
        }

        public void startMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().startAction(this);
            GetComponent<Fighter>().cancel();
            moveTo(destination);
        }

        public void moveTo(Vector3 destination)
        {
            agent.SetDestination(destination);
            agent.isStopped = false;
        }

        public void stop()
        {
            agent.isStopped = true;
        }
    }
}