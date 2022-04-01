using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    private bool _faddingOut = false;
    private Light _directionalLight;
    private Light _sunLight;
    public GameObject pauseCanvas;
    public GameObject practiceCanvas;
    [NonSerialized]
    public bool Paused = false;

    public GameObject pauseHelp;

    private bool _activeHelp = false;
    private float _countDownHelp = 0f;

    private bool _pauseHelpShown = false;
    

    void Start()
    {
        _directionalLight = GameObject.Find("/Directional Light").GetComponent<Light>();
        _sunLight = GameObject.Find("/Sun").GetComponent<Light>();
        practiceCanvas.SetActive(PlaySession.isPractice);
        if (PlaySession.isPractice) _directionalLight.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        _handlePause();
        _handleGameOver();
        _handleHelp();
    }

    public void GameOver()
    {
        _faddingOut = true;
        _directionalLight.color = Color.red;
    }

    public void GameBeaten()
    {
        SceneManager.LoadScene("Congratulations");
    }

    /// <summary>
    /// Change the tone of red in the light to give a sense of urgency
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

    public void Pause()
    {
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
        Paused = !Paused;
        Time.timeScale = Math.Abs(Time.timeScale - 1f) < 0.2 ? 0 : 1f;
    }

    public void ShowHelp(GameObject help)
    {
        if (_activeHelp) return;
        help.SetActive(true);
        _countDownHelp = 6f;
    }

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

    private void _handleGameOver()
    {
        if (!_faddingOut) return;
        var oldColor = _sunLight.color.r;
        // color values range 0 - 1. To gradually fade out in 5 sec we divide delta time by 5.
        var newColor = Mathf.Max(0, oldColor - Time.deltaTime/5);
        if (newColor <= 0) _loadGameOverScene();
        _sunLight.color = new Color(newColor, newColor, newColor);
        _directionalLight.color = new Color(newColor, 0, 0);
    }

    private void _loadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

}
