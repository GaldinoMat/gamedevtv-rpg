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

        float timeSinceLastAttack = 0;

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
            if (timeSinceLastAttack >= attackSpeed)
            {
                // Triggers the animation event
                anim.SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }


        // Animation event 
        void Hit()
        {
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= weaponRange;
        }

        public void attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().startAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            anim.SetTrigger("stopAttack");
        }
    }
}