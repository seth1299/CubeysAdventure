using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckController : MonoBehaviour
{
    /*

    "ParticleSystem" is the sucking in particle system which should be what you're attaching this script to. 

    "player" is the Player GameObject. 

    "other" is what the sucking animation (particle system) is colliding with (i.e. an enemy).

    */

    public ParticleSystem thisSameGameObject;

    public GameObject player;

    private Collider2D other = null;

    private EnemyController ECS = null;
    private BombController BCS = null;

    // All this script does is set an EnemyController variable to the "EnemyController" script attached to the enemy that it's sucking in and then set its
    // "setCanHurtPlayer" variable to false and then the enemy will start flying towards the player. 

    /*
    void Update()
    {
        if (!player.GetComponent<PlayerController>().GetHasEnemyInside())
        {
            if ( other != null)
            {
                if (other.CompareTag("Enemy"))
                {
                    ECS = other.gameObject.GetComponent<EnemyController>();
                }
                else if (other.CompareTag("Bomb"))
                {
                    BCS = other.gameObject.GetComponent<BombController>();
                }

                if (thisSameGameObject.isPlaying == true)
                {
                    if (other.CompareTag("Enemy") )
                    {
                        ECS.SetCanHurtPlayer(false);
                        ECS.StartCoroutine("getSuckedIn");
                    }
                    else if (BCS != null)
                    {
                        BCS.SetCanHurtPlayer(false);
                        BCS.StartCoroutine("getSuckedIn");
                    }
                }
            }
        }
    }

    */

    // This just sets the "other" variable to the enemy that this particle system touches.

    void OnTriggerEnter2D(Collider2D otherObject)
    {
        other = otherObject;
    }

    // This prevents the player from sucking in bombs from lightyears away

    void OnTriggerExit2D(Collider2D otherObject)
    {
        other = null;
    }

    public bool GetTouchingEnemy()
    {
        if (other == null)
            return false;
        else if (other.CompareTag("Enemy") && thisSameGameObject.isPlaying)
            return true;
        else
            return false;
    }
    

}
