using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "ProgressionObject", menuName = "Stats/New ProgressionObject", order = 0)]
    public class ProgressionObject : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClass = null;

        public float GetClassHealth(CharacterClass charClass, int level)
        {

            foreach (ProgressionCharacterClass classChild in characterClass)
            {
                if (classChild.characterClass == charClass)
                {
                    return classChild.health[level - 1];
                }
            }

            return 0;
        }

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public float[] health = null;
        }
    }
}
