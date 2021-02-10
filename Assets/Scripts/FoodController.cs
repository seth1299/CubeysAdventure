using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    /*

    Side note: Set the Animator Culling Mode to "cull completely" or else the animation will severely lag the game.

    "player" is the Player game object.

    "PCS" is the PlayerController script attached to the Player game object.

    "anim" is just an "Animator" that doesn't actually animate this object, it just switches between different sprites depending on what the food is tagged with.

    */
    public GameObject player;

    private PlayerController PCS = null;

    Animator anim;

    /*
    
    This just changes the appearance of the food to match what it's tagged with. This script only ones once, to not lag the game.

    The hamburger is when State = 1.
    The melon is when State = 2. 
    The Maximum Tomato is when State = 3. 
    The Juice Box is when State = 4. 
    The Donut is when State = 5.

    */
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();

        PCS = player.GetComponent<PlayerController>();

        if (gameObject.CompareTag("Burger"))
            anim.SetInteger("State", 1);
        else if (gameObject.CompareTag("Melon"))
            anim.SetInteger("State", 2);
        else if (gameObject.CompareTag("MaximumTomato"))
            anim.SetInteger("State", 3);
        else if (gameObject.CompareTag("JuiceBox"))
            anim.SetInteger("State", 4);
        else if (gameObject.CompareTag("Donut"))
            anim.SetInteger("State", 5);
    }

    // When colliding with the player, adds health depending on what kind of food it is. Healthier/heartier food such as the Maximum Tomato retsores all health,
    // Burgers and Melons restore 2 health, and juice boxes and donuts restore 1 health.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("MaximumTomato"))
                PCS.AddHealth(3);
            else if(gameObject.CompareTag("Burger") || gameObject.CompareTag("Melon"))
                PCS.AddHealth(2);
            else 
                PCS.AddHealth(1);
            
            Destroy(gameObject);
        }
    }
}
