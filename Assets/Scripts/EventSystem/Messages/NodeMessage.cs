using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeMessage : GameEventMessage
{
    [field: SerializeField] public Node Node { get; private set; }

    public NodeMessage(Node node)
    {
        Node = node;
    }

    public NodeMessage(string eventName, Node node) : base(eventName)
    {
        Node = node;
    }
}
