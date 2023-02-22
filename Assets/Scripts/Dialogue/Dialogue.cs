using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.RPGDialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        private void OnValidate()
        {
            nodeLookup.Clear();

            foreach (DialogueNode node in AllNodes)
            {
                nodeLookup[node.name] = node;
            }
        }

        public IEnumerable<DialogueNode> AllNodes => nodes;

        public DialogueNode RootNode => nodes[0];

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.Children)
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }
        }
#if UNITY_EDITOR

        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = System.Guid.NewGuid().ToString();
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");

            if (parent != null)
            {
                parent.AddChild(newNode.name);
            }

            Undo.RecordObject(this, "Added Dialogue Node");
            nodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode deletedNode)
        {
            Undo.RecordObject(this, ("Deleted Dialogue Node"));
            nodes.Remove(deletedNode);
            OnValidate();
            DeleteNodesChildren(deletedNode);
            Undo.DestroyObjectImmediate(deletedNode);
        }
        private void DeleteNodesChildren(DialogueNode deletedNode)
        {
            foreach (DialogueNode node in nodes)
            {
                node.RemoveChild(deletedNode.name);
            }
        }
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR

            if (nodes.Count == 0)
            {
                CreateNode(null);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in AllNodes)
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize() { }
    }
}