using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer launchBall;
    [SerializeField]
    private SpriteRenderer spareBall;
    [SerializeField]
    private GameObject singleBall;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private float shootingInterval;
    [SerializeField]
    private float shootingIntervalFromButton;

    private Color colorToLaunch;

    private bool isShootingPossible = false;
    private Vector3 tempPosition;
    private GameObject shootingBall;
    private Coroutine shootCoroutine;
    private Color tempColor;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            tempPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!IsPointerOutOfBounds(tempPosition) && isShootingPossible)
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

    private void OnMouseDown()
    {
        SwapBalls();
    }

    private void SwapBalls()
    {
        SoundsController.Get().PlaySwapSound();
        tempColor = launchBall.color;
        launchBall.color = spareBall.color;
        spareBall.color = tempColor;
    }

    private void LaunchBall()
    {
        if (isShootingPossible)
        {
            SoundsController.Get().PlayLaunchSound();
            isShootingPossible = false;
            SetShootingPossible(true);
            shootingBall = Instantiate(singleBall, launchBall.transform.position, Quaternion.identity);
            tempColor = launchBall.color;
            shootingBall.GetComponent<SpriteRenderer>().color = tempColor;
            shootingBall.GetComponent<Rigidbody2D>().AddForce((tempPosition - launchBall.transform.position).normalized * 1500, ForceMode2D.Force);
            launchBall.color = spareBall.color;
            spareBall.color = GridController.Get().GetAvailableColor();
        }
    }

    private bool IsPointerOutOfBounds(Vector3 position)
    {
        if (position.x < -3.8f || position.x > 3.8f || (position.x > -1f && position.x < 1f && position.y < -4.5f) || position.y < -5.4f || position.y >= 5.3f)
        {
            return true;
        }
        return false;
    }

    private IEnumerator DoShootDelayed(bool isFromButton = false)
    {
        if (isFromButton)
            yield return new WaitForSecondsRealtime(shootingIntervalFromButton);
        else
            yield return new WaitForSecondsRealtime(shootingInterval);
        isShootingPossible = true;
    }

    public void SetBallsReady()
    {
        launchBall.color = GridController.GetRandomColor();
        spareBall.color = GridController.GetRandomColor();
    }

    public void SetShootingPossible(bool isPossible)
    {
        if (isPossible)
        {
            if (shootCoroutine != null)
            {
                StopCoroutine(shootCoroutine);
                shootCoroutine = null;
            }
            shootCoroutine = StartCoroutine(DoShootDelayed());
        }
        else
            isShootingPossible = isPossible;
    }

    public void SetShootingPossibleFromButton()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
        shootCoroutine = StartCoroutine(DoShootDelayed(true));
    }

    public void SetShootingPossibleInstant(bool isPossible)
    {
        isShootingPossible = isPossible;
    }
}
