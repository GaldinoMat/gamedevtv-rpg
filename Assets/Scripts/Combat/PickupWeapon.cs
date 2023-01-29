using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class PickupWeapon : MonoBehaviour
    {
        [SerializeField] Weapon weaponPickup = null;

        [SerializeField] float respawnTime = 5f;

        [SerializeField] bool isRespawnable;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weaponPickup);

                if (isRespawnable)
                {
                    StartCoroutine(HideForSeconds(respawnTime));
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickUp(false);
            yield return new WaitForSeconds(seconds);
            ShowPickUp(true);
        }

        private void ShowPickUp(bool shouldShow)
        {
            gameObject.GetComponent<BoxCollider>().enabled = shouldShow;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}
