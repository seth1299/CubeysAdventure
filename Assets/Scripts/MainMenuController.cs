using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

// The StartGame() function loads the "GameScene" scene, which should be the actual game content.
public void StartGame()
{
    SceneManager.LoadScene("LevelOne");
}

// The HowToPlay() function loads the "HowToPlay" scene, which should contain the instructions for how to play the game.
public void HowToPlay()
{
    SceneManager.LoadScene("HowToPlay");
}

// The Credits() function loads the "Credits" scene, which should contain the credits for the game.
public void Credits()
{
    SceneManager.LoadScene("Credits");
}

// The MainMenu() function returns the user to the main menu.
public void MainMenu()
{
    SceneManager.LoadScene("MainMenu");
}

// This quits the game when the "exit" button is clicked.

public void Exit()
{
    Application.Quit();
}

void Update()
{
    // This code simply closes the application when the "ESC" key is pressed, as required in the "requirements for ALL games" section on Webcourses.
    if (Input.GetKey("escape"))
    {
        Application.Quit();
    }
}

}