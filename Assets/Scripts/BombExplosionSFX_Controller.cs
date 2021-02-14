using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionSFX_Controller : MonoBehaviour
{
    public AudioSource explosion;
    
    public void Explode()
    {
        explosion.Play();
    }
}
