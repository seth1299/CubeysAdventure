using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    /*

    "bossHealth" is how much health the boss has. In Kirby, this particular boss has 3 health.
    "index" is just an integer used for the purposes of a loop to count how many times the loop runs.
    "randNum" is how many times the boss jumps up and throws the bomb.

    "speed" is how fast the Boss can move.

    "anim" is an Animator component that handles the animations for the boss.

    "bomb" is the GameObject created by Instantiate(), i.e. what the boss throws out. Passing a Bomb gameObject will make him through bombs, whereas passing the Player
    gameObject will make him throw more players that you can control.

    */

    public int bossHealth = 3;
    private int index;
    private int randNum;

    private bool isJumping = false;

    public float speed;

    private Animator anim;

    public GameObject bomb;

    private Collider2D otherObject = null;

    public Transform isGroundedChecker;

    public LayerMask groundLayer;

    public AudioSource explosion;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine("Attack");
        //explosion = GetComponent<AudioSource>();
    }

    IEnumerator Attack()
    {

        anim.SetInteger("State", 0); //This is the idle animation
        yield return new WaitForSeconds(Random.Range(0.75f, 1.5f));
        anim.SetInteger("State", 1); //This is the walking animation
        randNum = Random.Range(80, 150);

        if (TouchingGround())
        {
        isJumping = true;
        for ( index = 0; index < randNum; index++ )
        {
            gameObject.transform.position = new Vector2 (transform.position.x, transform.position.y + (speed / 1.5f));
            yield return null;
        }
        isJumping = false;

        Destroy(Instantiate(bomb, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity ), 6f);
        }
        
        StartCoroutine("Attack");
    }

    public void Hurt()
    {
        bossHealth -= 1;
        if (bossHealth <= 0)
            Destroy(gameObject);
    }

    public int GetHealth()
    {
        return bossHealth;
    }

    void Update()
    {
        if (!TouchingGround() && !isJumping)
                gameObject.transform.position = new Vector2 (transform.position.x, ( transform.position.y - 0.025f ) );
    }

    private bool TouchingGround()
    {

    Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, 0.05f, groundLayer); 

    if (collider != null) 
        return true;

    else 
        return false;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (!other.CompareTag("Bomb"))
            {
                otherObject = other;
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        otherObject = null;
    }

    public void Explode()
    {
        explosion.Play();
    }
    
}
