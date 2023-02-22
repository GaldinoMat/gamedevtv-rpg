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

            float[] levels = lookup[charClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass charClass)
        {
            BuildLookup();

            float[] levels = lookup[charClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (lookup != null) return;

            lookup = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass classChild in characterClass)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in classChild.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                lookup[classChild.characterClass] = statLookupTable;
            }
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
