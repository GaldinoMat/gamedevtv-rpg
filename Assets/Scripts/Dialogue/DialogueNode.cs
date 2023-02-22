
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.RPGDialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        private bool isPlayerSpeaking = false; // Turn into ENUM when adding multiple speakers
        [SerializeField]
        private string text;
        [SerializeField]
        private List<string> children = new List<string>();
        [SerializeField]
        private Rect rectPosition = new Rect(0, 0, 200, 100);

        public Rect Rect => rectPosition;

        public string Text => text;

        public List<string> Children => children;

        public bool IsPlayerSpeaking => isPlayerSpeaking;

#if UNITY_EDITOR
        public void SetRect(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move dialogue node position");

            rectPosition.position = newPosition;

            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");

                text = newText;

                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");

            children.Add(childID);

            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");

            children.Remove(childID);

            EditorUtility.SetDirty(this);
        }

        public void SetSpeaker(bool isPlayerPeaker)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");

            isPlayerSpeaking = isPlayerPeaker;

            EditorUtility.SetDirty(this);
        }
#endif
    }
}