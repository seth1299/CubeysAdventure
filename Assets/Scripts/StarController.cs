using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    /*

    "speed" is how fast the Star moves. 
    "lastDirectionLooked" is either -1 or 1 and is the last direction that the player looked (either left or right, respectively)
    "player" is the Player GameObject.

    */

    public float speed;
    private float lastDirectionLooked;
    public GameObject player;
    public GameObject boss;

    // This just makes the Star fly left if the player was last facing left, otherwise the Star flies to the right if the player was last facing right.
    // The Star rotates to the left if the player was las facing left, otherwise the Star rotates to the right.


    // Awake() is only called when the Object is instantiated. That is, when the object is created. Setting the last direction looked only when the object is created
    // means that the star won't be constantly updating where it's supposed to be traveling, which not only saves on memory processing, but also prevents the player
    // from being able to change the direction of the star projectile by turning around.
    void Awake()
    {
        lastDirectionLooked = player.GetComponent<PlayerController>().GetLastDirectionLooked();
    }

    // This Update() function simply moves the Star projectile in the last direction that the player looked and makes it rotate.

    void Update()
    {

        if (lastDirectionLooked < 0)
        {
            gameObject.transform.position = new Vector2 ( transform.position.x - speed, transform.position.y );
        }
        else
        {
            gameObject.transform.position = new Vector2 ( transform.position.x + speed, transform.position.y );
        }

        if ( lastDirectionLooked < 0)
            transform.Rotate (new Vector3 (0, 0, -150) * Time.deltaTime);
        else
            transform.Rotate (new Vector3 (0, 0, 150) * Time.deltaTime);
    }

    // This just destroys the enemy and the star if the Star collides with an enemy.

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            other.gameObject.GetComponent<BossController>().Hurt();
            Destroy(gameObject);
        }
    }
}
