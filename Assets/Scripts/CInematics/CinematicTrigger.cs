using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool playedAnim = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !playedAnim)
            {
                GetComponent<PlayableDirector>().Play();
                playedAnim = true;
            }
        }
    }
}

