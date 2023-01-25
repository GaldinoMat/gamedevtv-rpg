using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField] float health;

        bool isDead = false;

        private void Start()
        {
            health = GetComponent<BaseStats>().GetNewStat(Stat.Health);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = MathF.Max(health - damage, 0);
            print(health);

            if (health <= 0)
            {
                AwardExperience(instigator);
                Die();
            }
        }

        public float GetPercentage()
        {
            return (health / GetComponent<BaseStats>().GetNewStat(Stat.Health)) * 100;
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience instigatorExp = instigator.GetComponent<Experience>();

            if (instigatorExp == null) return;

            instigatorExp.GainExperience(GetComponent<BaseStats>().GetNewStat(Stat.ExperienceReward));
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return health;
        }


        public void RestoreState(object state)
        {
            health = (float)state;

            if (health <= 0)
            {
                Die();
            }
        }
    }
}
