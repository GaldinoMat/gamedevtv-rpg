using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] GameObject[] destroyOnHit = null;

        [SerializeField] Health target = null;

        [SerializeField] float speed;
        [SerializeField] float lifeAfterImpact = 0;

        [SerializeField] bool isHoming = false;

        [SerializeField] GameObject hitEffect = null;

        [SerializeField] UnityEvent onHitProjectile;

        float damage = 0;

        GameObject instigator = null;

        private void Start()
        {
            if (!isHoming)
            {
                CheckTarget();
                transform.LookAt(GetAimPosition());
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isHoming)
            {
                CheckTarget();
                transform.LookAt(GetAimPosition());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.instigator = instigator;
            this.damage = damage;
        }

        public void CheckTarget()
        {
            if (target == null) return;
            if (target.IsDead())
            {
                Destroy(gameObject);
                return;
            }
        }

        private Vector3 GetAimPosition()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();

            if (targetCapsule == null) return target.transform.position;

            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;

            CheckTarget();

            target.TakeDamage(instigator, damage);

            speed = 0f;

            onHitProjectile.Invoke();

            if (hitEffect != null) Instantiate(hitEffect, GetAimPosition(), transform.rotation);

            if (destroyOnHit.Length > 0)
            {
                foreach (GameObject parts in destroyOnHit)
                {
                    Destroy(parts);
                }
                Destroy(gameObject, lifeAfterImpact);
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
}