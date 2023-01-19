using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{

    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        private void Start() {
            LoadInput();
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
        }

        public void LoadInput()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void SaveInput()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
    }

}