using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMessage : GameEventMessage
{
    public Node Node { get; private set; }

    public NodeMessage(Node node)
    {
        Node = node;
    }

    public NodeMessage(string eventName, Node node) : base(eventName)
    {
        Node = node;
    }
}
