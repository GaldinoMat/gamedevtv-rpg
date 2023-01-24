using UnityEngine;

namespace RPG.Effects
{
    public class DestroyEffectImpact : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
