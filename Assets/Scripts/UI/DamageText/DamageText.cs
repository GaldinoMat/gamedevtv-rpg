using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text damageText = null;

        public void SetText(float damage)
        {
            damageText.GetComponent<Text>().text = $"{damage}";
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}
