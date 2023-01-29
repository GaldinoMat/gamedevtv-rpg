using System;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] GameObject levelUpParticleEffect = null;

        [SerializeField] CharacterClass characterCLass;

        [SerializeField] ProgressionObject progressionObject = null;

        [SerializeField] bool isUsingModifiers = false;

        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;

        LazyValue<int> currentLevel;

        Experience experience = null;

        public event Action onLevelUp;

        private void Awake()
        {
            experience = GetComponent<Experience>();

            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetNewStat(Stat stat)
        {
            return Mathf.Floor((GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + (GetPercentageModifier(stat) / 100)));
        }

        private float GetBaseStat(Stat stat)
        {
            return progressionObject.GetStat(stat, characterCLass, GetLevel());
        }

        private float GetAdditiveModifiers(Stat stat)
        {
            float total = 0;
            if (isUsingModifiers)
            {
                foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
                {
                    foreach (float modifier in provider.GetAditiveModifiers(stat))
                    {
                        total += modifier;
                    }
                }
            }

            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            float total = 0;
            if (isUsingModifiers)
            {
                foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
                {
                    foreach (float modifier in provider.GetPercentageModifiers(stat))
                    {
                        total += Mathf.Ceil(modifier);
                    }
                }
            }

            return total;
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentExpPoints = experience.GetExperience();

            int penultimateLevel = progressionObject.GetLevels(Stat.ExperienceToLevelUp, characterCLass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = progressionObject.GetStat(Stat.ExperienceToLevelUp, characterCLass, level);
                if (xpToLevelUp > currentExpPoints)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}
