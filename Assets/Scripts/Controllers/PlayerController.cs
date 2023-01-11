using RPG.Movement;
using UnityEngine;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover mover;

        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
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

                if (!GetComponent<Fighter>().CanAttack(target)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().attack(target);
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