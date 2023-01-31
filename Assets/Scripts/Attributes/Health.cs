using System;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] UnityEvent takeDamage;

        LazyValue<float> health;

        bool isDead = false;

        private void Awake()
        {
            health = new LazyValue<float>(GetInitialHealth);
        }

        private void Start()
        {
            health.ForceInit();
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetNewStat(Stat.Health);
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += levelUpHealh;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= levelUpHealh;
        }

        private void levelUpHealh()
        {
            float regenHealth = GetComponent<BaseStats>().GetNewStat(Stat.Health) * (regenerationPercentage / 100);
            health.value = Mathf.Max(health.value, regenHealth);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + "took damage: " + damage);

            health.value = MathF.Max(health.value - damage, 0);

            if (health.value <= 0)
            {
                AwardExperience(instigator);
                Die();
            }
            else
            {
                takeDamage.Invoke();
            }
        }

        public string GetPercentage()
        {
            return $"{health.value} / {GetComponent<BaseStats>().GetNewStat(Stat.Health)}";
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
            return health.value;
        }


        public void RestoreState(object state)
        {
            health.value = (float)state;

            if (health.value <= 0)
            {
                Die();
            }
        }
    }
}
