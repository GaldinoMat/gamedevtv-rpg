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
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit item in hits)
            {
                CombatTarget target = item.transform.GetComponent<CombatTarget>();

                if (target == null) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    gameObject.GetComponent<Fighter>().Attack(target);
                }
                return true;
            }

            return false;
        }

        private bool interactWithMovement()
        {

            RaycastHit hit;
            bool isHit = Physics.Raycast(GetMouseRay(), out hit);

            if (isHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.moveTo(hit.point);
                }
                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}