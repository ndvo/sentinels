using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool _faddingOut = false;
    private Light _directionalLight;
    private Light _sunLight;

    void Start()
    {
        _directionalLight = GameObject.Find("/Directional Light").GetComponent<Light>();
        _sunLight = GameObject.Find("/Sun").GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
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

}
