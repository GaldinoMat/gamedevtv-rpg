using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        MonoBehaviour currentAction;

        public void startAction(MonoBehaviour action)
        {
            if (currentAction == action) return;

            if (currentAction != null)
            {
                print("Cancelling" + currentAction);
            }
            currentAction = action;
        }
    }
}