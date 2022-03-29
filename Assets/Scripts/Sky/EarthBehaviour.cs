using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sky
{

    public class EarthBehaviour : MonoBehaviour
    {
        public float resistance = 1000f;

        private Image _resistanceImage;

        private const float MAXResistance = 1000f;

        private const float ResistanceThreshold = 60f * 3f;

        public float survivedTime = 0f;

        private AudioSource _audioSource;

        private void Start()
        {
            _resistanceImage = GameObject.Find("/Canvas/Earth/MineralResources/Overlay").GetComponent<Image>();
            _audioSource = GetComponent<AudioSource>();
        }

        public void Update()
        {
            _handleEndGameScenarios();
            _handleRegeneration();
            _handleAlarm();
            _updateUi();
        }

        public float TakeDamage(float amount)
        {
            resistance -= amount;
            _updateUi();
            return amount;
        }

        private void _handleEndGameScenarios()
        {
            if (resistance <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }
            survivedTime += Time.deltaTime;
            if (survivedTime > ResistanceThreshold)
            {
                SceneManager.LoadScene("Congratulations");
            }
        }

        // Earth slowly regenerates itself.
        private void _handleRegeneration()
        {
            resistance = Mathf.Min(resistance + Time.deltaTime * 5, MAXResistance);
        }

        private void _handleAlarm()
        {
            if (resistance < 0.3f * MAXResistance)
            {
                if (_audioSource.isPlaying) return;
                _audioSource.Play();
            }
            else
            {
                _audioSource.Stop();
            }
        }

        private void _updateUi()
        {
            _resistanceImage.fillAmount = resistance / MAXResistance;
        }
    }
}