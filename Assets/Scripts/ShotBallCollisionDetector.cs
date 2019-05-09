using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBallCollisionDetector : MonoBehaviour
{
    private bool isCollided = false;

    public bool IsCollided { get => isCollided; set => isCollided = value; }
}
