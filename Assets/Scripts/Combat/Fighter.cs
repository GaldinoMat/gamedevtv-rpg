using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Health target;
        Mover mover;

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float attackSpeed;
        [SerializeField] float weaponDamage = 5f;

        float timeSinceLastAttack = Mathf.Infinity;

        Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;
            MoveToAttack();
        }

        public void MoveToAttack()
        {
            if (!GetIsInRange())
            {
                mover.moveTo(target.transform.position);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= attackSpeed)
            {
                // Triggers the animation event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            anim.ResetTrigger("stopAttack");
            anim.SetTrigger("attack");
        }


        // Animation event 
        void Hit()
        {
            if (target == null) return;

            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= weaponRange;
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null) return false;

            Health targetToAttack = target.GetComponent<Health>();
            return targetToAttack != null && !targetToAttack.IsDead();
        }

        public void attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().startAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            anim.ResetTrigger("attack");
            anim.SetTrigger("stopAttack");
        }
    }
}