using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] public AnimatorOverrideController weaponAnimatorOverride = null;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] float weaponRange = 0f;
        [SerializeField] float weaponDamage = 0f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyEquippedWeapon(rightHand, leftHand);

            if (weaponPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                GameObject newWeapon = Instantiate(weaponPrefab, handTransform);
                newWeapon.name = weaponName;
            }

            if (weaponAnimatorOverride != null)
                animator.runtimeAnimatorController = weaponAnimatorOverride;

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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance =
                Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }
    }
}