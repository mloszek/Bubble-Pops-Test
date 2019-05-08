using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsController : MonoBehaviour
{    
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



    private void OnApplicationQuit()
    {
        isShutingDown = true;
    }
}
