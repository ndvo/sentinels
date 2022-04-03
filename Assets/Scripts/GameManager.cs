using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
///     Game Manager controls general aspects of the game that are not related to specific objects.
/// </summary>
public class GameManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject practiceCanvas;

    public GameObject pauseHelp;

    private bool _activeHelp;
    private float _countDownHelp;
    private Light _directionalLight;

    private bool _fadingOut;

    private readonly bool _pauseHelpShown = false;
    private Light _sunLight;

    [NonSerialized] public bool Paused;


    private void Start()
    {
        _directionalLight = GameObject.Find("/Directional Light").GetComponent<Light>();
        _sunLight = GameObject.Find("/Sun").GetComponent<Light>();
        practiceCanvas.SetActive(PlaySession.isPractice);
        if (PlaySession.isPractice) _directionalLight.color = Color.blue;
    }

    // Update is called once per frame
    private void Update()
    {
        _handlePause();
        _handleGameOver();
        _handleHelp();
    }

    /// <summary>
    ///     Ends the game
    ///     Initiate a fading out sequence and set the light color to red.
    /// </summary>
    public void GameOver()
    {
        _fadingOut = true;
        _directionalLight.color = Color.red;
    }

    /// <summary>
    ///     Provide a function to return to the main menu.
    /// </summary>
    public void Quit()
    {
        SceneManager.LoadScene("MenuScene");
    }

    /// <summary>
    ///     Ends the game with the player's victory.
    /// </summary>
    public void GameBeaten()
    {
        SceneManager.LoadScene("Congratulations");
    }

    /// <summary>
    ///     Change the tone of red in the light to give a sense of urgency
    /// </summary>
    /// <param name="amount"></param>
    public void EmergencyLight(float amount)
    {
        amount = 1 - Mathf.Clamp(amount, 0, 1);
        var oldColor = _directionalLight.color;
        var reddish = Mathf.Clamp(oldColor.r - amount, 0, 1);
        _directionalLight.color = new Color(
            oldColor.r,
            reddish,
            reddish
        );
    }

    /// <summary>
    ///     Pauses the game
    ///     Makes the timescale equal to zero, causing any behaviour dependent on time not to happen.
    ///     It is important to note that Update is still called in GameObject instances, with zero Time.deltaTime.
    ///     FixedUpdate is not called at all.
    /// </summary>
    private void Pause()
    {
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
        Paused = !Paused;
        Time.timeScale = Math.Abs(Time.timeScale - 1f) < 0.2 ? 0 : 1f;
    }

    /// <summary>
    ///     Shows a provided help, which is provided as a game object.
    /// </summary>
    /// <param name="help"></param>
    public void ShowHelp(GameObject help)
    {
        if (_activeHelp) return;
        help.SetActive(true);
        _countDownHelp = 6f;
    }

    /// <summary>
    ///     Handles the active help.
    ///     There should be only one active help at a time and it should not be shown for more than _countDownHelp seconds.
    /// </summary>
    private void _handleHelp()
    {
        _activeHelp = practiceCanvas.transform.Cast<Transform>().Any(child => child.gameObject.activeSelf);
        if (_activeHelp) _countDownHelp = Math.Max(0, _countDownHelp - Time.deltaTime);
        if (_countDownHelp == 0 && _activeHelp)
        {
            practiceCanvas.transform.Cast<Transform>().First(c => c.gameObject.activeSelf).gameObject.SetActive(false);
            _activeHelp = false;
        }
    }

    /// <summary>
    ///     When paused, show the pause canvas and allow for unpause buttons to be triggered.
    /// </summary>
    private void _handlePause()
    {
        if (!Paused && !_pauseHelpShown && Random.value < 0.0005f)
            ShowHelp(pauseHelp);
        if (!Paused)
        {
            if (Input.GetButtonUp("Cancel")) Pause();
        }
        else
        {
            if (Input.GetButtonUp("Cancel") || Input.GetButtonUp("Jump") || Input.GetButtonUp("Fire1")) Pause();
        }
    }

    /// <summary>
    ///     Handles the process of ending the game.
    ///     There is a fading out process to end the game. Without this one would never see the player ship exploding and
    ///     the darkening of the scene.
    /// </summary>
    private void _handleGameOver()
    {
        if (!_fadingOut) return;
        var oldColor = _sunLight.color.r;
        // color values range 0 - 1. To gradually fade out in 5 sec we divide delta time by 5.
        var newColor = Mathf.Max(0, oldColor - Time.deltaTime / 5);
        if (newColor <= 0) _loadGameOverScene();
        _sunLight.color = new Color(newColor, newColor, newColor);
        _directionalLight.color = new Color(newColor, 0, 0);
    }

    private void _loadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }
}