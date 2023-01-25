using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterCLass;
        [SerializeField] ProgressionObject progressionObject = null;

        public float GetNewStat(Stat stat)
        {
            return progressionObject.GetStat(stat, characterCLass, startingLevel);
        }
    }
}
