using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    float PositionX { get; set; }
    float PositionY { get; set; }
    Color NodeColor { get; set; }
}
