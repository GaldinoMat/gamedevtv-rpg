using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterCLass;
        [SerializeField] ProgressionObject progressionObject = null;

        public float GetHealth()
        {
            return progressionObject.GetClassHealth(characterCLass, startingLevel);
        }
    }
}
