using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "ProgressionObject", menuName = "Stats/New ProgressionObject", order = 0)]
    public class ProgressionObject : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClass = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookup = null;

        public float GetStat(Stat stat, CharacterClass charClass, int level)
        {
            BuildLookup();

            //TODO: Implement lookup table search methods

            // foreach (ProgressionCharacterClass classChild in characterClass)
            // {
            //     if (classChild.characterClass != charClass) continue;

            //     foreach (ProgressionStat progressionStat in classChild.stats)
            //     {
            //         if (progressionStat.stat != stat) continue;

            //         if (progressionStat.levels.Length < level) continue;

            //         return progressionStat.levels[level - 1];
            //     }
            // }

            return 0;
        }

        private void BuildLookup()
        {
            if (lookup != null) return;

            //TODO: Implement lookup buildup
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }

}
