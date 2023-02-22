using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.RPGDialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0)
            {
                CreateNode(null);
            }
        }
#endif

        private void OnValidate()
        {
            nodeLookup.Clear();

            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.children)
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }
        }

        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = System.Guid.NewGuid().ToString();
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");

            if (parent != null)
            {
                parent.children.Add(newNode.name);
            }

            nodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode deletedNode)
        {
            nodes.Remove(deletedNode);
            OnValidate();
            DeleteNodesChildren(deletedNode);
            Undo.DestroyObjectImmediate(deletedNode);
        }

        private void DeleteNodesChildren(DialogueNode deletedNode)
        {
            foreach (DialogueNode node in nodes)
            {
                node.children.Remove(deletedNode.name);
            }
        }
    }
}