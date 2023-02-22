
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.RPGDialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        private string text;
        [SerializeField]
        private List<string> children = new List<string>();
        [SerializeField]
        private Rect rectPosition = new Rect(0, 0, 200, 100);

        public Rect Rect => rectPosition;

        public string Text => text;

        public List<string> Children => children;

#if UNITY_EDITOR
        public void SetRect(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move dialogue node position");

            rectPosition.position = newPosition;
        }

        public void SetText(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");

                text = newText;
            }
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");

            children.Add(childID);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");

            children.Remove(childID);
        }
#endif
    }
}