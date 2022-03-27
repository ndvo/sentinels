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

        private void Start()
        {
            _resistanceImage = GameObject.Find("/Canvas/Earth/MineralResources/Overlay").GetComponent<Image>();
        }

        public void Update()
        {
            resistance = Mathf.Clamp(resistance, 0, MAXResistance);
            if (resistance == 0)
            {
                SceneManager.LoadScene("GameOver");
            }
            survivedTime += Time.deltaTime;
            if (survivedTime > ResistanceThreshold)
            {
                SceneManager.LoadScene("Congratulations");
            }
            // Earth slowly regenerates itself.
            resistance += Time.deltaTime * 2;
            _updateUi();
        }

        public void TakeDamage(float amount)
        {
            resistance -= amount;
            _updateUi();
        }

        private void _updateUi()
        {
            _resistanceImage.fillAmount = resistance / MAXResistance;
        }
    }
}