using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            return new SerializeVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializeVector3 position = (SerializeVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            if (string.IsNullOrEmpty(serializedProperty.stringValue))
            {
                serializedProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}
