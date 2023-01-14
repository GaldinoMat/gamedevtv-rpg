using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField]
        Transform target;

        NavMeshAgent agent;

        Animator anim;

        Health health;

        [SerializeField] float maxSpeed = 6f;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            agent.enabled = !health.IsDead();

            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            float localZ = transform.InverseTransformDirection(velocity).z;

            anim.SetFloat("forwardSpeed", localZ);
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            agent.SetDestination(destination);
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            agent.isStopped = false;
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }
    }
}