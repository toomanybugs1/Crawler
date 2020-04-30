using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private List<Node> childrenNodes;

    public List<Node> ChildrenNodes { get => childrenNodes; }

    public bool Visited { get; set; }

    public Vector2Int BottomLeftCorner { get; set; }
    public Vector2Int BottomRightCorner { get; set; }
    public Vector2Int TopLeftCorner { get; set; }
    public Vector2Int TopRightCorner { get; set; }

    public int TreeLayerIndex { get; set; }
    public Node Parent { get; set; }

    public Node(Node parentNode)
    {
        childrenNodes = new List<Node>();
        this.Parent = parentNode;
        if (parentNode != null)
        {
            parentNode.AddChild(this);
        }
    }

    public void AddChild(Node node)
    {
        childrenNodes.Add(node);
    }

    public void RemoveChild(Node node)
    {
        childrenNodes.Remove(node);
    }
}
