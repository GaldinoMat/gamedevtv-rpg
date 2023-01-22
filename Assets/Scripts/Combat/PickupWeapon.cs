using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class PickupWeapon : MonoBehaviour
    {
        [SerializeField] Weapon weaponPickup = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weaponPickup);
                Destroy(gameObject);
            }
        }
    }
}
