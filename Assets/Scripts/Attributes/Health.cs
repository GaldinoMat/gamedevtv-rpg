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
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

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

        public void Heal(float healthToRestore)
        {
            health.value = Mathf.Min(health.value + healthToRestore, GetMaxHealthPoints());
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
            health.value = MathF.Max(health.value - damage, 0);

            if (health.value <= 0)
            {
                onDie.Invoke();
                AwardExperience(instigator);
                Die();
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public float GetHealthPoints()
        {
            return health.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetNewStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return health.value / GetComponent<BaseStats>().GetNewStat(Stat.Health);
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
