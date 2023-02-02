using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentlyActiveFade = null;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutOnLoad()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            return GenerateFade(1, time);
        }

        public IEnumerator FadeIn(float time)
        {
            return GenerateFade(0, time);
        }

        private IEnumerator GenerateFade(float fadeTarget, float time)
        {
            if (currentlyActiveFade != null)
            {
                StopCoroutine(currentlyActiveFade);
            }

            currentlyActiveFade = StartCoroutine(FadeRoutine(fadeTarget, time));

            yield return currentlyActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}

