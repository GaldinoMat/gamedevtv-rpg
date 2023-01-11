using RPG.Movement;
using UnityEngine;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover mover;

        Health health;
        
        Fighter fighter;

        private void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (interactWithCombat()) return;
            if (interactWithMovement()) return;
            print("Nothing here");
        }

        private bool interactWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(getMouseRay());

            foreach (RaycastHit item in hits)
            {
                CombatTarget target = item.transform.GetComponent<CombatTarget>();

                if (target == null) continue;

                if (!fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    fighter.attack(target.gameObject);
                }
                return true;
            }

            return false;
        }

        private bool interactWithMovement()
        {

            RaycastHit hit;
            bool isHit = Physics.Raycast(getMouseRay(), out hit);

            if (isHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.startMoveAction(hit.point);
                }
                return true;
            }

            return false;
        }

        private static Ray getMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}