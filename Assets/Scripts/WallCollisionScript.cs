using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        source.Play();
    }
}
