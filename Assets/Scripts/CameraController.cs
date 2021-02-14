using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// This script goes on the Main Camera and the "target" component is the game object that you want to follow (the player).

public class CameraController : MonoBehaviour
{
    // "target" is a game object which the camera will follow. Whatever game object you pass as the value, the camera will follow to the best of its ability.
    public GameObject target;

    // "boss" is the Boss game object.
    public GameObject boss;

    // "PCS" is just an abbreviation for "target.GetComponent<PlayerController>()", which is the PlayerController script associated with the "target" game object. 
    private PlayerController PCS = null;

    // "BCS" is just an abbreviation for "boss.GetComponent<BossController>()", which is the BossController script associated with the "boss" game object. 
    private BossController BCS = null;

    // "playerLives" is how many lives the player has, "playerHealth" is how much health the player has, and "bossHealth" is how much health the boss has.
    private int playerLives, playerHealth, bossHealth;

    // These are just the UI text for game over, health, lives, and boss health.
    public TextMeshProUGUI livesText;

    void Start()
    {
        PCS = target.GetComponent<PlayerController>();
        BCS = boss.GetComponent<BossController>();

        livesText.text = "";
    }

    // LateUpdate() is called after all of the other Update() functions have been called. A follow camera should always be implemented in LateUpdate because it tracks 
    // objects that might have moved inside Update.
    void LateUpdate()
    {
        // This line of code just moves the camera to where the target game object is.
        this.transform.position = new Vector3(target.transform.position.x, this.transform.position.y, this.transform.position.z);

        // This makes the game quit when pressing the "ESC" key, as detailed in the "Requirements for ALL Games" page on Webcourses.
        if (Input.GetKey("escape"))
            Application.Quit();
    }

    // Update just keepst track of the player's health, lives, and the boss's health.

    void Update()
    {
        if (PCS != null)
        {
            playerLives = PCS.GetLives();
            playerHealth = PCS.GetHealth();
        }
        else 
            playerLives = 0;
        
        if (BCS != null)
            bossHealth = BCS.GetHealth();
        else 
            bossHealth = 0;

        UpdateText();
    }

    // This just updates the text in the UI to match the corresponding variables.
    void UpdateText()
    {
        livesText.text = "Lives: " +playerLives;
        
        if (playerLives <= 0)
        {
            SceneManager.LoadScene("Lose");
        }
        else if (bossHealth <= 0)
        {
            SceneManager.LoadScene("Win");
        }
    }
}
