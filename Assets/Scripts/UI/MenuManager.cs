using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void LoadMainScene()
    {
        PlaySession.isPractice = false;
        SceneManager.LoadScene("MainScene");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadPracticeScene()
    {
        PlaySession.isPractice = true;
        SceneManager.LoadScene("MainScene");
    }
    
    
    
}
