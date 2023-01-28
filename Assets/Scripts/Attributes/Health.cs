using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField] float regenerationPercentage = 70;

        float health = -1f;

        bool isDead = false;


        private void Start()
        {
            GetComponent<BaseStats>().onLevelUp += levelUpHealh;

            if (health < 0)
            {
                health = GetComponent<BaseStats>().GetNewStat(Stat.Health);
            }

        }

        private void levelUpHealh()
        {
            float regenHealth = GetComponent<BaseStats>().GetNewStat(Stat.Health) * (regenerationPercentage / 100);
            health = Mathf.Max(health, regenHealth);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + "took damage: " + damage);

            health = MathF.Max(health - damage, 0);

            if (health <= 0)
            {
                AwardExperience(instigator);
                Die();
            }
        }

        public string GetPercentage()
        {
            return $"{health} / {GetComponent<BaseStats>().GetNewStat(Stat.Health)}";
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
