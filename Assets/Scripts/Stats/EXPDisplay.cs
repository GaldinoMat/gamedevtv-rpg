using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{

    public class EXPDisplay : MonoBehaviour
    {
        Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        // Update is called once per frame
        void Update()
        {
            gameObject.GetComponent<Text>().text = $"{experience.GetExperience()}";
        }
    }
}
