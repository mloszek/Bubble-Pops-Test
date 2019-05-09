using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Node parentNode;

    public void SetNode(Node node)
    {
        parentNode = node;
        node.Ball = gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<ShotBallCollisionDetector>().IsCollided)
        {
            parentNode.IsVisited = false;
            collision.gameObject.GetComponent<ShotBallCollisionDetector>().IsCollided = true;
            SoundsController.Get().PlayCollisionSound();
            GridController.Get().SnapBallAndUpdateView(parentNode.GetSnapNode(collision.gameObject.transform.position), collision.gameObject.GetComponent<SpriteRenderer>().color);
            Destroy(collision.gameObject);
        }        
    }
}
