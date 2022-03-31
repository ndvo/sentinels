using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{

    private bool _faddingOut = false;
    private Light _directionalLight;
    private Light _sunLight;
    public GameObject pauseCanvas;
    public GameObject practiceCanvas;
    [NonSerialized]
    public bool Paused = false;

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
        if (practiceCanvas.transform.Cast<Transform>().Any(child => child.gameObject.activeSelf))
            return;
        help.SetActive(true);
    }

    private void _handlePause()
    {
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
        // color values range 0 - 1. To gradually fade out in 5 sec we divide deltatime by 5.
        var newColor = oldColor - Time.deltaTime/5;
        if (newColor <= 0) _loadGameOverScene();
        _sunLight.color = new Color(newColor, newColor, newColor);
        _directionalLight.color = new Color(newColor, 0, 0);
    }

    private void _loadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    /// <summary>
    /// There can be only one help message on screen.
    /// </summary>
    private void _handleOnlyOneHelp()
    {
    }
    
}
