using System.Collections;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D
        }

        [SerializeField] Transform spawnPoint;

        [SerializeField] DestinationIdentifier destination;

        [SerializeField] int sceneToLoad = -1;

        [SerializeField] float fadeTimer = 3f;
        [SerializeField] float loadTimer = .5f;

        GameObject player = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();

            DisableControl();

            yield return fader.FadeOut(fadeTimer);

            wrapper.SaveInput();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            DisableControl();

            wrapper.LoadInput();

            Portal portal = GetOtherPortal();
            UpdatePlayer(portal);

            wrapper.SaveInput();

            yield return new WaitForSeconds(loadTimer);
            fader.FadeIn(fadeTimer);

            EnableControl();

            Destroy(gameObject, .1f);
        }

        void DisableControl()
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl()
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = true;
        }

        private void UpdatePlayer(Portal portal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(portal.spawnPoint.position);
            player.transform.rotation = portal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }

            return null;
        }
    }

}
