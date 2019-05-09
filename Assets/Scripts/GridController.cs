using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private Launcher launcher;
    [SerializeField]
    private UIController uIController;
    [SerializeField]
    private GameObject stationaryBall;
    [SerializeField]
    private GameObject destroyedBall;

    private static int level = 1;
    private static GridController instance;
    private static object _lock = new object();
    private static bool isShutingDown = false;
    private static int tempColorRange;

    private List<List<Node>> nodeRows = new List<List<Node>>();
    private GameObject tempBall;
    private List<Color> availableColors = new List<Color>();
    private Rigidbody2D tempRigidbody;

    private List<Node> ballsToDestroy = new List<Node>();
    private List<Node> ballsToFree = new List<Node>();

    private void Start()
    {
        CreateGrid();
        launcher.SetBallsReady();
    }

    public static GridController Get()
    {
        if (isShutingDown)
            return null;

        lock (_lock)
        {
            if (instance == null)
            {
                instance = (GridController)FindObjectOfType(typeof(GridController));

                if (instance == null)
                {
                    GameObject newGameObject = new GameObject(typeof(GridController).ToString());
                    instance = newGameObject.AddComponent<GridController>();
                    DontDestroyOnLoad(newGameObject);
                }
            }

            return instance;
        }
    }

    public void CreateGrid()
    {
        launcher.SetBallsReady();
        nodeRows.Clear();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 12; i++)
        {
            nodeRows.Add(new List<Node>());
            for (int j = 0; j < 7; j++)
            {
                if (i % 2 == 0)
                    nodeRows[i].Add(new Node(-3.1f + j * .95f, 5.4f - i * .9f, GetRandomColor()));
                else
                    nodeRows[i].Add(new Node(-2.625f + j * .95f, 5.4f - i * .9f, GetRandomColor()));
            }
        }

        InstantiateBalls();
        JoinNodes();
    }

    public void SnapBallAndUpdateView(Node node, Color color)
    {
        tempBall = null;
        tempBall = Instantiate(stationaryBall, gameObject.transform, true);
        tempBall.transform.position = new Vector2(node.PositionX, node.PositionY);
        tempBall.GetComponent<SpriteRenderer>().color = color;
        tempBall.AddComponent<BallController>().SetNode(node);
        node.IsTaken = true;
        node.NodeColor = color;
        node.IsVisited = false;
        node.IsFreeable = false;
        node.IsDestructable = false;

        UpdateView(node);
    }

    public Color GetAvailableColor()
    {
        availableColors.Clear();

        for (int i = 0; i < nodeRows.Count - 2; i++)
        {
            for (int j = 0; j < nodeRows[i].Count; j++)
            {
                if (nodeRows[i][j].IsTaken && !availableColors.Contains(nodeRows[i][j].NodeColor))
                    availableColors.Add(nodeRows[i][j].NodeColor);
            }
        }

        return availableColors[Random.Range(0, availableColors.Count)];
    }

    private void InstantiateBalls()
    {
        tempBall = null;

        for (int i = 0; i < nodeRows.Count - 5; i++)
        {
            for (int j = 0; j < nodeRows[i].Count; j++)
            {
                tempBall = Instantiate(stationaryBall, gameObject.transform, true);
                tempBall.transform.position = new Vector2(nodeRows[i][j].PositionX, nodeRows[i][j].PositionY);
                tempBall.GetComponent<SpriteRenderer>().color = nodeRows[i][j].NodeColor;
                tempBall.AddComponent<BallController>().SetNode(nodeRows[i][j]);
                nodeRows[i][j].IsTaken = true;
            }
        }
    }

    private void JoinNodes()
    {
        for (int i = 0; i < nodeRows.Count; i++)
        {
            for (int j = 0; j < nodeRows[i].Count; j++)
            {
                if (i % 2 == 0)
                {
                    nodeRows[i][j].NodeNW = i == 0 ? null : j == 0 ? null : nodeRows[i - 1][j - 1];
                    nodeRows[i][j].NodeNE = i == 0 ? null : nodeRows[i - 1][j];
                    nodeRows[i][j].NodeE = j == nodeRows[i].Count - 1 ? null : nodeRows[i][j + 1];
                    nodeRows[i][j].NodeSE = i == nodeRows.Count - 1 ? null : nodeRows[i + 1][j];
                    nodeRows[i][j].NodeSW = i == nodeRows.Count - 1 ? null : j == 0 ? null : nodeRows[i + 1][j - 1];
                    nodeRows[i][j].NodeW = j == 0 ? null : nodeRows[i][j - 1];
                }
                else
                {
                    nodeRows[i][j].NodeNW = i == 0 ? null : nodeRows[i - 1][j];
                    nodeRows[i][j].NodeNE = i == 0 ? null : j == nodeRows[i].Count - 1 ? null : nodeRows[i - 1][j + 1];
                    nodeRows[i][j].NodeE = j == nodeRows[i].Count - 1 ? null : nodeRows[i][j + 1];
                    nodeRows[i][j].NodeSE = i == nodeRows.Count - 1 ? null : j == nodeRows[i].Count - 1 ? null : nodeRows[i + 1][j + 1];
                    nodeRows[i][j].NodeSW = i == nodeRows.Count - 1 ? null : nodeRows[i + 1][j];
                    nodeRows[i][j].NodeW = j == 0 ? null : nodeRows[i][j - 1];
                }
                nodeRows[i][j].FillNodesList();
            }
        }
    }

    private void UpdateView(Node node)
    {
        ballsToDestroy.Clear();
        CheckForColorHit(node);
        MarkSameColorBalls(node);
        DestroySameColorBalls();
        ResetVisiting();
        CheckIfWin();
        CheckIfLoose();
    }

    private void CheckForColorHit(Node parentNode)
    {
        foreach (Node node in parentNode.GetNeighbourNodes())
        {
            if (node != null && node.IsTaken && node.NodeColor == parentNode.NodeColor)
            {
                ballsToDestroy.Add(parentNode);
                break;
            }
        }
    }

    private void MarkSameColorBalls(Node parentNode)
    {
        parentNode.IsVisited = true;
        foreach (Node node in parentNode.GetNeighbourNodes())
        {
            if (node != null && node.IsTaken && !node.IsVisited && node.NodeColor == parentNode.NodeColor)
            {
                ballsToDestroy.Add(node);
                MarkSameColorBalls(node);
            }
        }
    }

    private void DestroySameColorBalls()
    {
        if (ballsToDestroy.Count > 0)
        {
            SoundsController.Get().PlayDestructionSound();
        }

        uIController.SetScoreText(ballsToDestroy.Count * 10 * level);

        foreach (Node node in ballsToDestroy)
        {
            node.IsTaken = false;
            Destroy(Instantiate(destroyedBall, node.Ball.gameObject.transform.position, Quaternion.identity), .75f);
            Destroy(node.Ball);
            node.Ball = null;
        }

        ballsToDestroy.Clear();
    }

    private void ResetVisiting()
    {
        ballsToFree.Clear();

        for (int i = 0; i < nodeRows.Count - 6; i++)
        {
            for (int j = 0; j < nodeRows[i].Count; j++)
            {
                nodeRows[i][j].IsVisited = false;
            }
        }
    }

    private void CheckIfWin()
    {
        for (int i = 0; i < nodeRows.Count - 6; i++)
        {
            for (int j = 0; j < nodeRows[i].Count; j++)
            {
                if (nodeRows[i][j] != null && nodeRows[i][j].IsTaken)
                    return;
            }
        }

        launcher.SetShootingPossibleInstant(false);
        uIController.SetWinScreen();
        level += 1;
    }

    private void CheckIfLoose()
    {
        foreach (Node node in nodeRows[11])
        {
            if (node != null && node.IsTaken)
            {
                launcher.SetShootingPossibleInstant(false);
                uIController.SetLooseScreen();
                level = 1;
                break;
            }
        }        
    }

    private bool IsBallFree(Node parentNode)
    {
        foreach (Node node in parentNode.GetNeighbourNodes())
        {
            if (node == null || (node != null && node.IsTaken))
                return false;
        }
        return true;
    }

    public static Color GetRandomColor()
    {
        if (level == -1)
            tempColorRange = 3;
        else if (level >= 0 && level < 3)
            tempColorRange = 4;
        else if (level >= 3 && level < 7)
            tempColorRange = 5;
        else if (level >= 7 && level < 12)
            tempColorRange = 6;
        else
            tempColorRange = 7;

        switch (Random.Range(0, tempColorRange))
        {
            case 0:
                return Color.blue;
            case 1:
                return Color.cyan;
            case 2:
                return Color.magenta;
            case 3:
                return Color.red;
            case 4:
                return Color.yellow;
            case 5:
                return Color.green;
            case 6:
                return Color.gray;
            default:
                return Color.gray;
        }
    }

    private void OnApplicationQuit()
    {
        isShutingDown = true;
    }
}
