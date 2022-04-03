using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Helper class that provide functions to be used by the menu buttons
/// </summary>
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

    public void LoadStoryScene()
    {
        SceneManager.LoadScene("Story");
    }
}