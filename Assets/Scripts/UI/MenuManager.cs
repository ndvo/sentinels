using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    ///     Helper class that provide functions to be used by the menu buttons
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        public void LoadMainScene()
        {
            PlaySession.IsPractice = false;
            SceneManager.LoadScene("MainScene");
        }

        public void LoadMenuScene()
        {
            SceneManager.LoadScene("MenuScene");
        }

        public void LoadPracticeScene()
        {
            PlaySession.IsPractice = true;
            SceneManager.LoadScene("MainScene");
        }

        public void LoadStoryScene()
        {
            SceneManager.LoadScene("Story");
        }
    }
}