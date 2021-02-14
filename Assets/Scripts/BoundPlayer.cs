using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundPlayer : MonoBehaviour
{
    public GameObject player;
    private PlayerController PCS;

    void Start()
    {
        PCS = player.GetComponent<PlayerController>();
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "LevelOne")
        {
            if (PCS.GetXPosition() < -8.47f)
            {
                player.transform.position = new Vector2 (-8.4f, player.transform.position.y );
            }
            else if (PCS.GetXPosition() > 100f)
            {
                player.transform.position = new Vector2 (100f, player.transform.position.y );
            }
        }
        else
        {
            if (PCS.GetXPosition() < 0.05f)
            {
                player.transform.position = new Vector2 (0.05f, player.transform.position.y );
            }
            else if (PCS.GetXPosition() > 92f)
            {
                player.transform.position = new Vector2 (92f, player.transform.position.y );
            }
        }
    }
}
