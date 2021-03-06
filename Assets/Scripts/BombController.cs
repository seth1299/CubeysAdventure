﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{

    public GameObject player;
    public float speed;

    private PlayerController PCS;

    //private bool isDestroyed = false;
    private bool canHurtPlayer = true;

    void Start()
    {
        PCS = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        gameObject.transform.position = new Vector2 ( transform.position.x - speed, transform.position.y );
        transform.Rotate (new Vector3 (0, 0, -90) * Time.deltaTime);
    }

    IEnumerator getSuckedIn()
    {
        // "playerXPos" just gets the X position of the "player" GameObject attached to this script, and "playerYPos" gets the y position of the same GameObject. 
        // "PCS" is the aforementioned PlayerController at the beginning of the script that is just shorthand for "player.GetComponent<PlayerController>()". 
        // "PCS" is just significantly easier to type than that repeatedly.

        float playerXPos = PCS.GetXPosition(), playerYPos = PCS.GetYPosition();

        // This while loop checks if the enemy isn't at the same X and Y position as the player (since sucking them in will bring the enemy to the player's position)
        // and if the player is currently sucking in. If all three conditions are true, then the while loop is executed and will continue to execute as long as all
        // three conditions are true.

        while ( transform.position.x != playerXPos && transform.position.y != playerYPos && PCS.GetIsSucking() )
        {
            // This code just makes the enemy move very slowly towards the player's position while the player is sucking them in.

            gameObject.transform.position = new Vector2 (transform.position.x + ( ( playerXPos - transform.position.x ) / 200), transform.position.y + ( ( playerYPos - transform.position.y ) / 200));
            
            // "yield return null" simply means "wait a frame". This means that this script slowly happens repeatedly over time instead of just instantly changing the
            // X and Y positions of the enemy without smoothly moving them towards the player.

            yield return null;
        }

        // This tells Cubey that he has an enemy inside of him so he can't eat any more enemies.

        PCS.SetHasEnemyInside(true);

        yield return null;

        // This tells the player that the enemy they sucked in was destroyed so that the player knows that they successfully swallowed an enemy.

        //isDestroyed = true;
        Destroy(gameObject);

    }

    // This sets the "canHurtPlayer" boolean to whatever value is passed to the function (either true or false).

    public void SetCanHurtPlayer(bool value)
    {
        canHurtPlayer = value;
    }

    // This tells the player if the enemy can hurt him or not.

    public bool GetCanHurtPlayer()
    {
        return canHurtPlayer;
    }

}
