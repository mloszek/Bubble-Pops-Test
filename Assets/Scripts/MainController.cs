using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField]
    private Launcher launcher;
    [SerializeField]
    private Collider2D topDisabler;
    [SerializeField]
    private Collider2D bottomDisabler;
    [SerializeField]
    private BallPuller puller;

    private void Start()
    {
        GridController.Get().CreateGrid();
    }
}
