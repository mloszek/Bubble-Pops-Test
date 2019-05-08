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

    public bool isShootingPossible = false;
    private Vector3 tempPosition;
    private GameObject shootingBall;
    private Coroutine setShootingCoroutine;

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
            else
                lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!IsPointerOutOfBounds(tempPosition))
            {
                LaunchBall();
            }
        }
    }

    private void LaunchBall()
    {
        if (isShootingPossible)
        {
            shootingBall = Instantiate(singleBall, launchPivot.transform.position, Quaternion.identity);
            shootingBall.GetComponent<Rigidbody2D>().AddForce((tempPosition - launchPivot.transform.position).normalized * 2000, ForceMode2D.Force);
        }
    }

    private bool IsPointerOutOfBounds(Vector3 position)
    {
        if (position.x < -3.8f || position.x > 3.8f || position.y < -5.4f || position.y >= 5f)
        {
            return true;
        }
        return false;
    }

    private IEnumerator SetShootingDelayed()
    {
        yield return new WaitForSecondsRealtime(1);
        isShootingPossible = true;
    }

    public void SetShootingPossible(bool isPossible)
    {
        if (isPossible)
        {
            if (setShootingCoroutine != null)
            {
                StopCoroutine(setShootingCoroutine);
                setShootingCoroutine = null;
            }
            setShootingCoroutine = StartCoroutine(SetShootingDelayed());
        }
        else
            isShootingPossible = isPossible;
    }

    public void SetShootingPossibleInstant(bool isPossible)
    {
        isShootingPossible = isPossible;
    }
}
