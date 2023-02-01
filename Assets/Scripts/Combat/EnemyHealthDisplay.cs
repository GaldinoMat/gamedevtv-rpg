using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter playerFighter;

        private void Awake()
        {
            playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (playerFighter.GetTarget() == null)
            {
                gameObject.GetComponent<Text>().text = "N/A";
            }
            else
            {
                Health enemyHealth = playerFighter.GetTarget();
                gameObject.GetComponent<Text>().text = $"{enemyHealth.GetHealthPoints()}/{enemyHealth.GetMaxHealthPoints()}";
                
                // OR
                // gameObject.GetComponent<Text>().text = String.Format("{0:0.0}%", enemyHealth.GetPercentage());
            }
        }
    }
}