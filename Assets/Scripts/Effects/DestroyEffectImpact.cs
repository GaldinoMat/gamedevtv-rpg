using UnityEngine;

namespace RPG.Effects
{
    public class DestroyEffectImpact : MonoBehaviour
    {
        [SerializeField] GameObject target = null;
        // Update is called once per frame
        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if (target != null)
                {
                    Destroy(target);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
