using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.SceneManagement;  
public class SceneChange: MonoBehaviour {  
    public void Scene1() {  
        SceneManager.LoadScene("MainMenu");  
    }  
    public void Scene2() {  
        SceneManager.LoadScene("HowToPlay");  
    }  
    public void Scene3() {  
        SceneManager.LoadScene("Credits");  
    }  
    public void Scene4() {  
        SceneManager.LoadScene("LevelOne");  
    }  
} 