using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySpacePartitioner
{
    public RoomNode rootNode;

    public BinarySpacePartitioner(int dungeonWidth, int dungeonLength)
    {
        this.rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);
    }

    public List<RoomNode> PrepareNodesCollection(int maxPasses, int roomMinWidth, int roomMinLength)
    {
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> listToReturn = new List<RoomNode>();
        graph.Enqueue(this.rootNode);
        listToReturn.Add(this.rootNode);

        int i = 0;
        while (i < maxPasses && graph.Count > 0)
        {
            i++;

            RoomNode currentNode = graph.Dequeue();
            if (currentNode.Width >= roomMinWidth*2 || currentNode.Length >= roomMinLength*2)
            {
                SplitTheSpace(currentNode, listToReturn, roomMinLength, roomMinWidth, graph);
            }
        }
        return listToReturn;
    }

    private void SplitTheSpace(RoomNode currentNode, List<RoomNode> listToReturn, int roomMinLength, int roomMinWidth, Queue<RoomNode> graph)
    {
        Line line = GetLineDividingSpace(currentNode.BottomLeftCorner, currentNode.TopRightCorner, roomMinWidth, roomMinLength);

        RoomNode node1, node2;
        if (line.Orientation == Orientation.Horizontal)
        {
            node1 = new RoomNode(currentNode.BottomLeftCorner, 
                new Vector2Int(currentNode.TopRightCorner.x, line.Coordinates.y), 
                currentNode, currentNode.TreeLayerIndex + 1);

            node2 = new RoomNode(new Vector2Int(currentNode.BottomLeftCorner.x, line.Coordinates.y), 
                currentNode.TopRightCorner, 
                currentNode, currentNode.TreeLayerIndex + 1);
        } else
        {
            node1 = new RoomNode(currentNode.BottomLeftCorner,
                new Vector2Int(line.Coordinates.x, currentNode.TopRightCorner.y),
                currentNode, currentNode.TreeLayerIndex + 1);

            node2 = new RoomNode(new Vector2Int(line.Coordinates.x, currentNode.BottomLeftCorner.y),
                currentNode.TopRightCorner,
                currentNode, currentNode.TreeLayerIndex + 1);
        }

        AddNewNodeToCollection(listToReturn, graph, node1);
        AddNewNodeToCollection(listToReturn, graph, node2);
    }

    private void AddNewNodeToCollection(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node)
    {
        listToReturn.Add(node);
        graph.Enqueue(node);
    }

    private Line GetLineDividingSpace(Vector2Int bottomLeftCorner, Vector2Int topRightCorner, int roomMinWidth, int roomMinLength)
    {
        Orientation orientation;
        bool lengthStatus = (topRightCorner.y - bottomLeftCorner.y >= 2 * roomMinLength);
        bool widthStatus = (topRightCorner.x - bottomLeftCorner.x >= 2 * roomMinWidth);
        if (lengthStatus && widthStatus)
        {
            orientation = (Orientation)(UnityEngine.Random.Range(0, 2));
        } else if (widthStatus) {
            orientation = Orientation.Vertical;
        } else
        {
            orientation = Orientation.Horizontal;
        }

        return new Line(orientation, GetCoordinatesForOrientation(orientation, bottomLeftCorner, topRightCorner, roomMinWidth, roomMinLength));
    }

    private Vector2Int GetCoordinatesForOrientation(Orientation orientation, Vector2Int bottomLeftCorner, Vector2Int topRightCorner, int roomMinWidth, int roomMinLength)
    {
        Vector2Int coordinates = Vector2Int.zero;
        if (orientation == Orientation.Horizontal)
        {
            coordinates = new Vector2Int(0, UnityEngine.Random.Range(bottomLeftCorner.y + roomMinLength, topRightCorner.y - roomMinLength));
        } else
        {
            coordinates = new Vector2Int(UnityEngine.Random.Range(bottomLeftCorner.x + roomMinWidth, topRightCorner.x - roomMinWidth), 0);
        }
        return coordinates;
    }
}
