using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : INode
{
    private float _positionX;
    private float _positionY;
    private Color _nodeColor;
    private GameObject _ball;

    private bool isTaken;
    private bool isVisited;
    private bool isFreeable;
    private bool isDestructable;

    private Node nodeNW = null;
    private Node nodeNE = null;
    private Node nodeE = null;
    private Node nodeSE = null;
    private Node nodeSW = null;
    private Node nodeW = null;

    private static Color defaultColor = Color.white;
    private List<Node> nodesList = new List<Node>();
    private Vector2 nodePosition;
    private float distance;
    private float tempDistance;
    private Node tempNode;

    public Node(float positionX, float positionY, Color nodeColor)
    {
        _positionX = positionX;
        _positionY = positionY;
        _nodeColor = nodeColor;
        isTaken = false;
        isVisited = false;
        isFreeable = false;
        isDestructable = false;

        nodePosition = new Vector2(positionX, positionY);
    }

    #region Properties
    
    public float PositionX { get => _positionX; set => _positionX = value; }
    public float PositionY { get => _positionY; set => _positionY = value; }
    public Color NodeColor { get => _nodeColor; set => _nodeColor = value; }
    public GameObject Ball { get => _ball; set => _ball = value; }

    public bool IsTaken { get => isTaken; set => isTaken = value; }
    public bool IsVisited { get => isVisited; set => isVisited = value; }
    public bool IsFreeable { get => isFreeable; set => isFreeable = value; }
    public bool IsDestructable { get => isDestructable; set => isDestructable = value; }

    public Node NodeNW { get => nodeNW; set => nodeNW = value; }
    public Node NodeNE { get => nodeNE; set => nodeNE = value; }
    public Node NodeE { get => nodeE; set => nodeE = value; }
    public Node NodeSE { get => nodeSE; set => nodeSE = value; }
    public Node NodeSW { get => nodeSW; set => nodeSW = value; }
    public Node NodeW { get => nodeW; set => nodeW = value; }

    #endregion

    public void FillNodesList()
    {
        nodesList.Add(nodeNW);
        nodesList.Add(nodeNE);
        nodesList.Add(nodeE);
        nodesList.Add(nodeSE);
        nodesList.Add(nodeSW);
        nodesList.Add(nodeW);
    }

    public List<Node> GetNeighbourNodes()
    {
        return nodesList;
    }

    public Node GetSnapNode(Vector2 otherNodePos)
    {
        distance = 0;

        nodesList.RemoveAll(x => x == null);

        for (int i = 0; i < nodesList.Count; i++)
        {
            if (i == 0 && nodesList[i] != null)
                tempNode = nodesList[i];
            else if (nodesList[i] != null)
            {
                distance = Vector2.Distance(nodesList[i].nodePosition, otherNodePos);

                if (tempNode != null && distance < Vector2.Distance(tempNode.nodePosition, otherNodePos) && !nodesList[i].isTaken)
                    tempNode = nodesList[i];
            }
        }

        return tempNode;
    }
}
