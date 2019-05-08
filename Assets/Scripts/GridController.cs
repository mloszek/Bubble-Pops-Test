using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private GameObject stationaryBall;

    private static GridController instance;
    private static object _lock = new object();
    private static bool isShutingDown = false;

    List<List<Node>> nodeRows = new List<List<Node>>();

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
        for (int i = 0; i < 6; i++)
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

    private void InstantiateBalls()
    {
        GameObject tempNode;

        for (int k = 0; k < nodeRows.Count; k++)
        {
            for (int l = 0; l < nodeRows[k].Count; l++)
            {
                tempNode = Instantiate(stationaryBall, gameObject.transform, true);
                tempNode.transform.position = new Vector2(nodeRows[k][l].PositionX, nodeRows[k][l].PositionY);
                tempNode.GetComponent<SpriteRenderer>().color = nodeRows[k][l].NodeColor;
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
            }
        }
    }

    private Color GetRandomColor()
    {
        switch (Random.Range(0, 7))
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
                return Color.white;
        }
    }

    private void OnApplicationQuit()
    {
        isShutingDown = true;
    }
}
