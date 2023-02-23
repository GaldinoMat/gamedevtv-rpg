using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{

    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        [SerializeField] float fadeTime = 1f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadOnStart(defaultSaveFile);
            Fader fade = FindObjectOfType<Fader>();

            fade.FadeOutOnLoad();
            yield return fade.FadeIn(fadeTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveInput();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadInput();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeleteSave();
            }
        }

        public void LoadInput()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void SaveInput()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void DeleteSave()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }

}