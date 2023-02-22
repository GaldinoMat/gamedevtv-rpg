using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] public AnimatorOverrideController weaponAnimatorOverride = null;

        [SerializeField] Weapon weaponPrefab = null;

        [SerializeField] Projectile projectile = null;

        [SerializeField] float weaponRange = 0f;
        [SerializeField] float weaponDamage = 0f;
        [SerializeField] float weaponPercentageBonus = 0f;


        [SerializeField] bool isRightHanded = true;

        const string weaponName = "Weapon";

        public Weapon SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyEquippedWeapon(rightHand, leftHand);

            Weapon newWeapon = null;

            if (weaponPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                newWeapon = Instantiate(weaponPrefab, handTransform);
                newWeapon.gameObject.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (weaponAnimatorOverride != null)
            {
                animator.runtimeAnimatorController = weaponAnimatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return newWeapon;
        }

        private void DestroyEquippedWeapon(Transform rightHand, Transform leftHand)
        {
            Transform equippedWeapon = rightHand.Find(weaponName);

            if (equippedWeapon == null)
            {
                equippedWeapon = leftHand.Find(weaponName);
            }

            if (equippedWeapon == null) return;

            equippedWeapon.name = "Destroyed Weapon";
            Destroy(equippedWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        public bool IsProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance =
                Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetPercentageBonus()
        {
            return weaponPercentageBonus;
        }
    }
}