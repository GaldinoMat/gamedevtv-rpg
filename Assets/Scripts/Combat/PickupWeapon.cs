using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class PickupWeapon : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weaponPickup = null;

        [SerializeField] float respawnTime = 5f;

        [SerializeField] bool isRespawnable;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weaponPickup);

            if (isRespawnable)
            {
                StartCoroutine(HideForSeconds(respawnTime));
            }
            else
            {
                Destroy(gameObject);
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

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
