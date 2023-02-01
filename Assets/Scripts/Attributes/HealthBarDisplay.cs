using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBarDisplay : MonoBehaviour
    {
        [SerializeField] Health health;
        [SerializeField] RectTransform healthTransform;
        [SerializeField] Canvas canvasObj;


        // Update is called once per frame
        void Update()
        {
            if (Mathf.Approximately(health.GetFraction(), 0) || Mathf.Approximately(health.GetFraction(), 1))
            {
                canvasObj.enabled = false;
                return;
            }
            canvasObj.enabled = true;
            healthTransform.localScale = new Vector3(health.GetFraction(), 1, 1);
        }
    }
}