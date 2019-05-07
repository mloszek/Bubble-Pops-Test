using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField]
    private GameObject launchPivot;
    [SerializeField]
    private GameObject singleBall;
    [SerializeField]
    private LineRenderer lineRenderer;

    private Vector3 tempPosition;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            tempPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!IsPointerOutOfBounds(tempPosition))
            {
                lineRenderer.SetPosition(1, tempPosition);
                lineRenderer.enabled = true;
            }            
        }
        else
        {
            lineRenderer.enabled = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!IsPointerOutOfBounds(tempPosition))
            {
                Debug.LogError((tempPosition - launchPivot.transform.position).normalized);
                var go = Instantiate(singleBall, launchPivot.transform.position, Quaternion.identity);
                go.GetComponent<Rigidbody2D>().AddForce((tempPosition - launchPivot.transform.position).normalized * 2000,  ForceMode2D.Force);
            }            
        }
    }

    private bool IsPointerOutOfBounds(Vector3 position)
    {
        if (position.x < -3.8f || position.x > 3.8f || position.y < -5.4f)
        {
            return true;
        }
        return false;
    }
}
