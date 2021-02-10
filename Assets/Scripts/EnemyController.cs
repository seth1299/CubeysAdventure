using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /*

    "speed" is how fast the enemy goes.
    "randomWait" is how long the enemy waits before walking again.
    "randomDirection" is either -1 (left) or 1 (right).

    "index" is a basic index for loops.

    "canHurtPlayer" checks if the enemy can hurt the player (i.e. if the player isn't sucking them in / invincible).
    "isDestroyed" checks if this enemy is destroyed.

    "player" is the "Player" GameObject. 

    "PCS" is the PlayerController Script (PCS) that is attached to the "Player" GameObject. This is just for easier-to-read code.

    "anim" is an Animator component attached to the enemy that handles the "Idle" and "Walking" animations.
    
    */

    public float speed;
    public float randomWaitMinimumValue;
    public float randomWaitMaximumValue;
    private float randomWait;
    private float randomDirection;

    private int index;

    private bool canHurtPlayer = true;
    private bool isDestroyed = false;

    public GameObject player;

    private PlayerController PCS;

    Animator anim;

    // Start is called at the beginning of the game.
	// All of the variables are initialized at the start of the game.

    void Start()
    {
        PCS = player.GetComponent<PlayerController>();
        index = 0;
        anim = GetComponent<Animator>();
        speed = 0.25f * PCS.GetSpeed();
        randomDirection = Random.Range(-1.0f, 1.0f);

        // This is where "randomDirection" is set to either -1 or 1.
        if (randomDirection <= 0)
            randomDirection = -1;
        else
            randomDirection = 1;

        // This makes the enemy start randomly walking around.
        StartCoroutine("RandomlyWalkAround");
    }

    // This makes the enemy randomly walk around and pause to play the idle animation.
    IEnumerator RandomlyWalkAround()
    {
        // This animation code simply makes the enemy switch between idle and walking animations.

        anim.SetInteger("State", 0); //This is the idle animation
        yield return new WaitForSeconds(randomWait);
        anim.SetInteger("State", 1); //This is the walking animation

        // This sets the "randomWait" variable to a number between the minimum and maximum numbers passed via the Inspector.

        randomWait = Random.Range(randomWaitMinimumValue, randomWaitMaximumValue);
        randomWait = Mathf.Round(randomWait);

        // This sets the random direction that the enemy walks in to either -1 or 1. If it's -1, then the speed will be negatively affected and move left, otherwise
        // if it's 1 then the enemy will move to the right.

        randomDirection = Random.Range(-1.0f, 1.0f);

        if (randomDirection <= 0)
            randomDirection = -1;
        else
            randomDirection = 1;

        // This for loop just makes the enemy move a very small amount of space over 200 frames in rapid succession to simulate Rigidbody.AddForce() without needing to
        // use Rigidbody.AddForce().

        for ( index = 0; index < 200; index++ )
        {
            gameObject.transform.position = new Vector2 (transform.position.x + (randomDirection * speed), transform.position.y );
            yield return null;
        }

        // This is an infinitely recusive call to this same function. Normally this would not be recommended as infinite recursion can usually crash systems, but
        // nothing in this script adds or multiplies numbers together to make larger numbers. Instead, the same variables are just repeatedly changed to the same
        // repeating values, so no lag is added due to this infinite recursion.

        StartCoroutine("RandomlyWalkAround");
    }

    // getSuckedIn handles the enemy getting sucked in by the player. 

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
            gameObject.transform.position = new Vector2 (transform.position.x + ( ( playerXPos - transform.position.x ) / 400), transform.position.y + ( ( playerYPos - transform.position.y ) / 1.05f));
            
            // "yield return null" simply means "wait a frame". This means that this script slowly happens repeatedly over time instead of just instantly changing the
            // X and Y positions of the enemy without smoothly moving them towards the player.

            yield return null;
        }

        // This tells Cubey that he has an enemy inside of him so he can't eat any more enemies.

        PCS.SetHasEnemyInside(true);

        yield return null;

        // This tells the player that the enemy they sucked in was destroyed so that the player knows that they successfully swallowed an enemy.

        isDestroyed = true;
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

    // This just returns the "isDestroyed" variable, which can be either true or false.

    public bool GetDestroyed()
    {
        return isDestroyed;
    }
    
}
