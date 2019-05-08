using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : INode
{
    private float _positionX;
    private float _positionY;
    private Color _nodeColor;

    private bool isVisited;
    private bool isDestructable;
    private bool isFreeable;

    private Node nodeNW = null;
    private Node nodeNE = null;
    private Node nodeE = null;
    private Node nodeSE = null;
    private Node nodeSW = null;
    private Node nodeW = null;

    private static Color defaultColor = Color.white;

    public Node(float positionX, float positionY, Color nodeColor)
    {
        _positionX = positionX;
        _positionY = positionY;
        _nodeColor = nodeColor;
        IsVisited = false;
        IsDestructable = false;
        IsFreeable = false;
    }

    #region Properties
    
    public float PositionX { get => _positionX; set => _positionX = value; }
    public float PositionY { get => _positionY; set => _positionY = value; }
    public Color NodeColor { get => _nodeColor; set => _nodeColor = value; }

    public bool IsVisited { get => isVisited; set => isVisited = value; }
    public bool IsDestructable { get => isDestructable; set => isDestructable = value; }
    public bool IsFreeable { get => isFreeable; set => isFreeable = value; }

    public Node NodeNW { get => nodeNW; set => nodeNW = value; }
    public Node NodeNE { get => nodeNE; set => nodeNE = value; }
    public Node NodeE { get => nodeE; set => nodeE = value; }
    public Node NodeSE { get => nodeSE; set => nodeSE = value; }
    public Node NodeSW { get => nodeSW; set => nodeSW = value; }
    public Node NodeW { get => nodeW; set => nodeW = value; }

    #endregion
}
