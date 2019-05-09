using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsController : MonoBehaviour
{
    [SerializeField]
    private AudioSource collisionSound;
    [SerializeField]
    private AudioSource destructionSound;
    [SerializeField]
    private AudioSource swapSound;
    [SerializeField]
    private AudioSource launchSound;

    private static SoundsController instance;
    private static object _lock = new object();
    private static bool isShutingDown = false;

    public static SoundsController Get()
    {
        if (isShutingDown)
            return null;

        lock (_lock)
        {
            if (instance == null)
            {
                instance = (SoundsController)FindObjectOfType(typeof(SoundsController));

                if (instance == null)
                {
                    GameObject newGameObject = new GameObject(typeof(SoundsController).ToString());
                    instance = newGameObject.AddComponent<SoundsController>();
                    DontDestroyOnLoad(newGameObject);
                }
            }

            return instance;
        }
    }

    public void PlayCollisionSound()
    {
        collisionSound.Play();
    }

    public void PlayDestructionSound()
    {
        destructionSound.Play();
    }

    public void PlaySwapSound()
    {
        swapSound.Play();
    }

    public void PlayLaunchSound()
    {
        launchSound.Play();
    }

    private void OnApplicationQuit()
    {
        isShutingDown = true;
    }
}
