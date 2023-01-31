using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;

        public void Spawn(float damageAmount)
        {
            damageTextPrefab.SetText(damageAmount);
            DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
        }

    }
}
