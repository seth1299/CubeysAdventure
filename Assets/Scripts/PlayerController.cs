using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
	/*
	
    This script goes on the player's game object.

    Misc notes: 
    
    - This script works the best when the following statistics are used:
      * "speed" variable in the PlayerController script is set to "0.075".
      * "checkGroundRadius" variable in the PlayerController script is set to "0.05".
      * "isGroundedChecker" parameter in the PlayerController script is set to an empty game object that is a child of the player, located directly on the very bottom of 
        the player.
      * "Ground Layer" is set to the Layer that the ground is tagged with.
      * A BoxCollider2D component is attached to the player with "is trigger" turned on.


    These are all of the variables that will be used in the script.

    "speed" is the speed at which the player moves.
    "knockback" is how far the player gets knocked back when an enemy or projectile hits them. 
    "horizontal" is if the player is facing left or right. 
    "vertical" is if the player is jumping or ducking. 
    "checkGroundRadius" is the radius of which the "isGroundedChecker" looks to find the ground. 
    "checkBlockRadius" is the radius of which the "blockChecker" looks to find the block.
    "lastDirectionLooked" is the last direction that the player looked, -1 for left and 1 for right.
    "Door1_X_Axis" is the X axis that the first door is located on. 
    "Door1_Y_Axis" is the Y axis that the first door is located on. 
    "Door2_X_Axis" is the X axis that the second door is located on. 
    "Door2_Y_Axis" is the Y axis that the second door is located on. 

    "health" is the amount of hits the player can take before dying and losing a life. 
    "startingHealth" is the amount of health the player started with, so that the player's health can be reset to that value after a life is lost.
    "lives" is the amount of total lives the player has before it's game over. 
    "index" is just used for the looRightSideSuck in this script, and almost always is set to 0 and changes throughout the loop. 
    "targetDoor" is which door the player goes to.
    "numAllowedEnemiesToSwallow" is how many enemies the player can swallow. This int is probably outdated at this point because I replaced it with the boolean 
    "hasEnemyInside". 


    "isGrounded" is if the player is in the air or on the ground. 
    "canJumpAnymore" checks if the player is allowed to jump anymore. Basically this just means if the player is hitting the boundary of the game or not. 
    "launchedOffGround" checks if the player has jumped off of the ground. 
    "notFalling" checks to see if the player is falling. 
    "canBeHurt" is if the player can currently be damaged by enemies or projectiles. 
    "canEnterDoor" is if the player is standing on top of a door or not. 
    "enteringDoor" is if the player is currently entering a door. Useful for the Animation to play. 
    "gettingHurt" is if the player is currently taking damage. Useful for the hurt Animation to play. 
    "isSucking" checks to see if the player is currently sucking.
    "leftSuckActive" checks to see if the player is currently sucking in on the left side of the player.
    "rightSuckActive" checks to see if the player is currently sucking in on the right side of the player.
    "hasEnemyInside" checks to see if the player has an enemy inside of them.

    "isGroundedChecker" is just an empty game object underneath the player that checks if the player is touching the ground. 
    "blockChecker" is just an empty game object on top of the player that checks if the player is touching the block.

    "groundLayer" is the layer on which the Ground is located on.
    "blockLayer" is the layer on which the Block is located on.

    "anim" is the Animator component on the Player, which handles all of the animations that the player can go through.

    The TextMeshProUGUI's are just the text that displays the amount of health, lives, and whether or not the player lost the game.

    The particle systems are just a left side suck and a right side suck. One goes on the left and one goes on the right of Cubey. 

    "other" is just a Collider2D that is useful for checking what the player is colliding with. 

    "star" is the star projectile that Cubey shoots out after swallowing an enemy. 
    "boss" is the boss GameObject. 

    */
    public float speed;
    private float knockback;
    private float horizontal;
    private float vertical;
    public float checkGroundRadius = 0.05f; 
    public float checkBlockRadius = 0.05f;
    private float lastDirectionLooked;
    public float Door1_X_Axis;
    public float Door1_Y_Axis;
    public float Door2_X_Axis;
    public float Door2_Y_Axis;

    public int health;
    private int startingHealth;
    public int lives;
    private int index;
    private int targetDoor = 2;
    public int numAllowedEnemiesToSwallow = 1;
    private int currentLevel = 1;
    

    private bool isGrounded = false; 
    private bool canJumpAnymore = true;
    private bool launchedOffGround = false;
    private bool notFalling = true;
    private bool canBeHurt;
    private bool canEnterDoor;
    private bool enteringDoor;
    private bool gettingHurt = false;
    private bool isSucking = false;
    private bool leftSuckActive = false;
    private bool rightSuckActive = false;
    public bool hasEnemyInside = false;
    private bool justSpitOut = false;

    public Transform isGroundedChecker; 
    public Transform blockChecker;
    
    public LayerMask groundLayer;
    public LayerMask blockLayer;
    public LayerMask doorLayer;

    Animator anim;

    public ParticleSystem RightSideSuck;
    public ParticleSystem LeftSideSuck;

    private Collider2D other = null;

    public GameObject star;
    public GameObject boss;

    public AudioSource succ;

    // Start is called before the first frame update.
    // All of the uninitialized variables become initialized here.
    void Start()
    {
        succ = gameObject.GetComponent<AudioSource>();
        succ.Stop();
        RightSideSuck.Pause();
        RightSideSuck.Clear();
        LeftSideSuck.Pause();
        LeftSideSuck.Clear();
        anim = GetComponent<Animator>();
        index = 0;
        knockback = speed;
        canBeHurt = true;
        canEnterDoor = false;
        enteringDoor = false;
        startingHealth = health;
    }

    // Update is called once per frame.
    void Update()
    {

        // Two float variables "horiztonal" and "vertical" are set to the direction the player is facing.
        // "horizontal" will be -1 if the player is facing left and 1 if the player is facing right.
        // "vertical" will be -1 if the player pressed down or S and 1 if the player pressed up or W.

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // This line of code simply moves the player left and right. That's all it does.

        if ( !leftSuckActive && !rightSuckActive )
            gameObject.transform.position = new Vector2 (transform.position.x + (horizontal * speed), transform.position.y );

        // These are all calls to functions for checking if the player is on the ground, then check if the player wants to jump, then check if there's a block above
        // the player, then if the player isn't holding the jump key and is in the air then they start falling, then it checks if the player is touching an enemy.
        
        GetEnemyCanHurt();

        Suck();

        Animate();

        GetGrounded();

        StandingOnBlock();

        Jump();

        GetBlock();

        Fall();
        
        if (currentLevel == 1)
            targetDoor = 2;
        else
            targetDoor = 1;
        
        

        // This line of code continuously checks if the player is pressing E or not, and if they are then it runs the "EnterDoor" Coroutine.

        if (Input.GetKey(KeyCode.E))
        {
            StartCoroutine("EnterDoor");
        }
        if (Input.GetKeyDown(KeyCode.Q))
            succ.Play();
        if (Input.GetKeyUp(KeyCode.Q))
            succ.Stop();

        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow)))
            lastDirectionLooked = -1f;
        else if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow)))
            lastDirectionLooked = 1f;

    }


    // The Jump() function lets the player jump into the air and the player can stay there indefinitely, just like in the Kirby games.

    void Jump()
    {
        if ( ( Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) ) && canJumpAnymore )
        {
            notFalling = true;
            anim.SetInteger("State", 4); //This is the float "animation" where it's just thicc Kirby
            gameObject.transform.position = new Vector2 (transform.position.x, transform.position.y + 0.1f);
        }
    }

    // The GetGrounded() function simply checks to see if the player is on the ground or not. If the player is on the ground, then "isGrounded" is set to true.
    // Otherwise, "isGrounded" is set to false (meaning the player is in the air).

    void GetGrounded() 
    { 

    Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer); 

    if (collider != null) 
    { 
        isGrounded = true;
        launchedOffGround = false;
    } 
    else 
    { 
        isGrounded = false; 
        launchedOffGround = true;
    } 

    }

    void StandingOnBlock() 
    { 

    Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, blockLayer); 

    if (collider != null) 
    { 
        isGrounded = true;
        launchedOffGround = false;
    } 
    else 
    { 
        isGrounded = false; 
        launchedOffGround = true;
    } 

    }

    // The GetBlock function just checks if the player is touching the block.

    void GetBlock() 
    { 

    Collider2D collider2 = Physics2D.OverlapCircle(blockChecker.position, checkBlockRadius, blockLayer); 

    if (collider2 != null) 
    { 
        canJumpAnymore = false;
    } 
    else 
    { 
        canJumpAnymore = true; 
    } 

    }

    // GetIsGrounded just returns the "isGrounded" variable.

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    // GetSpeed just returns the "speed" variable.

    public float GetSpeed()
    {
        return speed;
    }

    // Fall Checks to see if the player isn't pressing down on the jump key and if the player is not on the ground. If both of those conditions are met, then the
    // player will start slowly descending to the ground.

    void Fall()
    {
        if ( ! ( ( Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) ) && canJumpAnymore )  && !isGrounded)
        {
            notFalling = false;
            anim.SetInteger("State", -2); //This is for falling down while in the air
            gameObject.transform.position = new Vector2 (transform.position.x, ( transform.position.y - 0.05f ) );
        }
    }

    // Animate handles changing the player's animations.

    void Animate()
    {
        if ( gettingHurt )
            anim.SetInteger("State", 5); //This is the "kirby hurt" animation state
        else if ( (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ) && !launchedOffGround && !isSucking)
            anim.SetInteger("State", 1); //This is for running right
        else if ( ( Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) ) && !launchedOffGround && !isSucking)
            anim.SetInteger("State", -1); //This is for running left
        else if ( ( Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) ) && !launchedOffGround && !isSucking)
        {
            anim.SetInteger("State", 2); //This is for the initial jump
            launchedOffGround = true;
        }
        else if ( ( Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) ) && isGrounded && !isSucking)
            anim.SetInteger("State", 3); //This is for ducking while the player is on the ground
        else if ( launchedOffGround && notFalling && !isSucking)
            anim.SetInteger("State", 4); //This is the thicc Kirby in the air animation
        else if ( enteringDoor )
            anim.SetInteger("State", 6); //This is the entering door animation
        else if ( rightSuckActive )
            anim.SetInteger("State", 7); //This is sucking right animation
        else if ( leftSuckActive )
            anim.SetInteger("State", -7); //This is sucking left animation
        else
            anim.SetInteger("State", 0); //This is the idle animation
    }

    // This OnTriggerEnter2D function makes the player get hurt when they walk into an enemy or an enemy projectile.

    void OnTriggerEnter2D(Collider2D other)
    {
        if ( other != null)
        {
            if ( ( other.CompareTag("Enemy") && canBeHurt))
            {
                if ( other.gameObject.GetComponent<EnemyController>().GetCanHurtPlayer() )
                {
                    if ( transform.position.x <= other.transform.position.x)
                    {
                        knockback = Mathf.Abs(knockback);
                    }
                    else
                    {
                        knockback = -1 * Mathf.Abs(knockback);
                    }
                    StartCoroutine("PlayerHurt");
                }
            }
            else if ( other.CompareTag("Bomb") && canBeHurt)
            {
                if ( other.gameObject.GetComponent<BombController>().GetCanHurtPlayer() )
                {
                    if ( transform.position.x <= other.transform.position.x)
                    {
                        knockback = Mathf.Abs(knockback);
                    }
                    else
                    {
                        knockback = -1 * Mathf.Abs(knockback);
                    }
                    StartCoroutine("PlayerHurt");
                }
            }
            else if ( other.CompareTag("Door1"))
            {
                canEnterDoor = true;
            }
            else if ( other.CompareTag("Door2"))
            {
                canEnterDoor = true;
            }
            else if ( other.CompareTag("Platform") )
                canJumpAnymore = false;
            else if ( other.CompareTag("Invincibility"))
                StartCoroutine("Invincibility");
            
        }
            
    }

    // Ensures that the player can't enter the door unless they are actually physically touching the door.

    void OnTriggerExit2D(Collider2D other)
    {
        if ( other.CompareTag("Door1") && targetDoor == 1 && !enteringDoor)
        {
            // This space intentionally left blank. 
        }
        else if (other.CompareTag("Door2") && targetDoor == 2 && !enteringDoor)
        {
            // This space intentionally left blank. 
        }
        else if (other.CompareTag("Door1") || other.CompareTag("Door2"))
        {
            canEnterDoor = false;
        }
        else if ( other.CompareTag("Platform") )
        {
            canJumpAnymore = true;
        }
        else
        {
            
        }
        
    }

    IEnumerator ExitDoor()
    {
        Collider2D collider3 = Physics2D.OverlapCircle(blockChecker.position, checkBlockRadius, doorLayer); 
        while (collider3 != null)
        {
            yield return null;
        }
        //canEnterDoor = false;
    }
    
    // EnterDoor checks if the player can enter the door, then if they are allowed to, makes the player temporarily invincible (supposedly), tells the Animator that
    // the player is entering a door, waits a second, and then teleports the player to a new location on the map and tells the Animator that they aren't going through
    // a door anymore.

    IEnumerator EnterDoor()
    {
            if ( canEnterDoor )
                {
                    canBeHurt = false;
                    enteringDoor = true;
                    yield return new WaitForSeconds(.6f);
                    
                    if (targetDoor == 2)
                    {
                        currentLevel = 2; //55, -2.119f
                        gameObject.transform.position = new Vector2(Door2_X_Axis, Door2_Y_Axis);
                    }
                    else
                    {
                        currentLevel = 1; //-2.356956f, 2.27f
                        gameObject.transform.position = new Vector2(Door1_X_Axis, Door1_Y_Axis);
                    }
                    
                    yield return new WaitForSeconds(0.001f);
                }

            canBeHurt = true;
            enteringDoor = false;
            StopCoroutine("EnterDoor");
    }

    // PlayerHurt just tells the Animator that the player is getting hurt, damages the player, and handles the loss of health accordingly.
    
    public IEnumerator PlayerHurt()
    {
        // This tells the player that it can't currently be hurt again because it is currently being hurt (i.e. temporary invincibility until the knockback finishes)
        canBeHurt = false;
        gettingHurt = true;

        // This simulates Rigidbody.AddForce() but without needing a Rigidbody component. Basically, by utilizing "yield return null" to wait a frame every time it's
        // run, the player very slowly moves in the opposite direction that they're facing until the knockback is complete.
        for ( index = 0; index < 11; index++ )
        {
            gameObject.transform.position = new Vector2 ((transform.position.x - (4 * knockback) ), transform.position.y );
            yield return null;
        }

        // This makes sure that the player can be hurt again and that the player is not currently being hurt.
        canBeHurt = true;
        gettingHurt = false;

        // Health is subtracted by one when taking damage.
        health--;

        // If your health reaches or goes below 0, then you lose a life and your health is set back to max.
        if ( health <= 0 )
        {
            lives -= 1;
            health = startingHealth;
        }

        // Otherwise, if the player is out of lives, then the "GAME OVER" screen is displayed instead. **Might need to change this to LoadScene for UI purposes**.
        if (lives <= 0)
        {
            gameObject.SetActive(false);
        }
        
    }

    // The Suck() function handles the player sucking in enemies.
    void Suck()
    {
        
        
        // This script will only activate if the player doesn't have an enemy inside of them already. Otherwise, if they do have an enemy inside of them, they will
        // spit out a star in the direction the player is facing instead of sucking in an enemy.
        if ( !hasEnemyInside  && !justSpitOut)
        {
            
        
        // If the player is looking to the left, then Cubey sucks on the left side of him. Otherwise, Cubey sucks on the right side of him.
        if (lastDirectionLooked < 0)
        {
        
        // If the player is pressing down Q and Cubey is also not currently sucking on the right side of him, then the player will be told that the player is
        // sucking on the left side and the left side sucking animation will play, allowing the SuckController() script (not on this game object) to start working.
        if (Input.GetKey(KeyCode.Q) && !rightSuckActive && !hasEnemyInside)
        {
            leftSuckActive = true;
            isSucking = true;
            LeftSideSuck.Play();
        }

        // Otherwise, if the player isn't meeting any of that criteria, then stop sucking on the left side.
        else
        {
            leftSuckActive = false;
            isSucking = false;
            LeftSideSuck.Pause();
            LeftSideSuck.Clear();
        }

        }

        // Otherwise, if the player is looking to the right, then Cubey sucks on the right side of him. Otherwise, Cubey sucks on the left side of him.
        else 
        {
        
        // If the player is pressing down Q and Cubey is also not currently sucking on the left side of him, then the player will be told that the player is
        // sucking on the right side and the right side sucking animation will play, allowing the SuckController() script (not on this game object) to start working.
        if (Input.GetKey(KeyCode.Q) && !leftSuckActive && !hasEnemyInside)
        {
            rightSuckActive = true;
            isSucking = true;
            RightSideSuck.Play();
        }

        else
        {
            rightSuckActive = false;
            isSucking = false;
            RightSideSuck.Pause();
            RightSideSuck.Clear();
        }

        }

        }

        // This makes Cubey spit out a star if he has an enemy inside of him, destroying the star after 2 seconds to save performance.
        // It also tells the player that they no longer have an enemy inside of them so they can suck enemies again.
        else if (Input.GetKeyDown(KeyCode.Q) && hasEnemyInside)
        {
            justSpitOut = true;
            Destroy(Instantiate(star, new Vector3(GetXPosition(), GetYPosition(), transform.position.z), Quaternion.identity ), 2f);
            StartCoroutine("SpitOut");
            SetHasEnemyInside(false);
            SetNumAllowedEnemiesToSwallow(1);
        }

        // Otherwise, if none of the other criteria is met, stop Cubey from sucking on both sides.
        else
        {
            succ.Stop();
            leftSuckActive = false;
            isSucking = false;
            rightSuckActive = false;
            RightSideSuck.Pause();
            RightSideSuck.Clear();
            LeftSideSuck.Pause();
            LeftSideSuck.Clear();
        }

        
        
    }
    // The SpitOut function ensures that while the player is holding down the Q button, nothing happens, but when they let go of it then "justSpitOut" is set
    // to false and the coroutine is stopped.

    public IEnumerator SpitOut()
    {
        while (Input.GetKey(KeyCode.Q))
        {
            yield return null;
        }
        justSpitOut = false;
        StopCoroutine("SpitOut");
    }

    // The Invincibility function just makes the player invincible for 5 seconds.

    public IEnumerator Invincibility()
    {
        canBeHurt = false;
        yield return new WaitForSeconds(5.0f);
        canBeHurt = true;
    }

    // This just returns the player's X position.
    public float GetXPosition()
    {
        return transform.position.x;
    }

    // This just returns the player's Y position.
    public float GetYPosition()
    {
        return transform.position.y;
    }

    // This just returns door 1's X axis.
    public float GetDoor1_X_Axis()
    {
        return Door1_X_Axis;
    }

    // This just returns door 1's Y axis.
    public float GetDoor1_Y_Axis()
    {
        return Door1_Y_Axis;
    }

    // This just returns door 2's X axis.
    public float GetDoor2_X_Axis()
    {
        return Door2_X_Axis;
    }

    // This just returns door 2's Y axis.
    public float GetDoor2_Y_Axis()
    {
        return Door2_Y_Axis;
    }
    
    // This just returns the "isSucking" variable, which can be either true or false.
    public bool GetIsSucking()
    {
        return isSucking;
    }

    // This just returns the "canHurtPlayer" variable from the Enemy's EnemyController script.
    public bool GetEnemyCanHurt()
    {
        if (other != null)
        {
            canBeHurt = other.gameObject.GetComponent<EnemyController>().GetCanHurtPlayer();
        }
        return canBeHurt;
    }

    // This just sets the "hasEnemyInside" variable to whatever is passed as the function argument.
    public void SetHasEnemyInside(bool parameter)
    {
        hasEnemyInside = parameter;
    }

    public bool GetHasEnemyInside()
    {
        return hasEnemyInside;
    }

    // This just sets the "numAllowedEnemiesToSwallow" variable to whatever is passed as the function argument.
    public void SetNumAllowedEnemiesToSwallow(int parameter)
    {
        numAllowedEnemiesToSwallow = parameter;
    }

    // This just returns the "numAllowedEnemiesToSwallow" variable.
    public int GetNumAllowedEnemiesToSwallow()
    {
        return numAllowedEnemiesToSwallow;
    }

    // This just returns the "horizontal" variable.
    public float GetHorizontal()
    {
        return horizontal;
    }

    // This simply tells whatever calls this function the last direction that the player looked (-1 for left, 1 for right).
    public float GetLastDirectionLooked()
    {
        return lastDirectionLooked;
    }

    // This returns how many lives the player has.
    public int GetLives()
    {
        return lives;
    }

    // This returns how much health the player has.
    public int GetHealth()
    {
        return health;
    }

    // This returns the current level the player is on.
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    // This just sets the current level to the specified parameter.
    public void SetCurrentLevel(int parameter)
    {
        currentLevel = parameter;
    }

    // This just sets if the player can enter a door or not. 
    public void SetCanEnterDoor(bool parameter)
    {
        canEnterDoor = parameter;
    }

    // This just gets if the player can enter a door or not. 
    public bool GetCanEnterDoor()
    {
        return canEnterDoor;
    }
    
    // This function adds health to the player, and if their health is over their maximum health, then their health is set to their maximum health.
    public void AddHealth(int value)
    {
        health += value;
        if (health > startingHealth)
            health = startingHealth;
    }

}
