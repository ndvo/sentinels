using UnityEngine;
using UnityEngine.UI;

namespace Sky
{
    /// <summary>
    ///     EarthBehaviour controls the the amount of damage Earth takes, updates the UI and handles visual and sound
    ///     effects.
    /// </summary>
    public class EarthBehaviour : MonoBehaviour
    {
        private const float ResistanceThreshold = 60f * 3f;
        public float resistance = 1000f;

        public float MAXResistance = 1000f;

        public float survivedTime;

        private float _accumulatedDamage;

        private AudioSource _audioSource;

        private GameManager _gameManager;

        private Image _resistanceImage;

        private void Start()
        {
            _gameManager = GameObject.Find("/GameManager").GetComponent<GameManager>();
            _resistanceImage = GameObject.Find("/Canvas/Earth/MineralResources/Overlay").GetComponent<Image>();
            _audioSource = GetComponent<AudioSource>();
            if (PlaySession.isPractice)
            {
                MAXResistance *= 10f;
                resistance *= 10f;
            }
        }

        public void Update()
        {
            _handleEndGameScenarios();
            _handleRegeneration();
            _handleAlarm();
            _updateUi();
        }

        /// <summary>
        ///     Takes and resets the accumulated damage.
        /// </summary>
        public void LateUpdate()
        {
            resistance -= Mathf.Clamp(_accumulatedDamage, 0, 25 * Time.deltaTime);
            _accumulatedDamage = 0f;
            // Changes the game lights to cause the impression of danger.
            _gameManager.EmergencyLight(resistance / MAXResistance);
            _updateUi();
        }

        /// <summary>
        ///     Takes a given amount of damage.
        ///     Earth has a cap on the amount of damage it can take per second.
        ///     Without this cap when a great number of ships attack Earth at the same time the game ends quickly. Lowering
        ///     the damage the ships can cause would render each enemy ship close to useless.
        ///     This solution makes it so that each enemy ship attacking Earth can cause damage, but yet if a dozer ships
        ///     hit at the same time the player still get a chance.
        /// </summary>
        /// <param name="amount">The amount of damage one intends to cause</param>
        /// <returns>the amount of damage to be taken into account, within a cap.</returns>
        public float TakeDamage(float amount)
        {
            _accumulatedDamage += amount;
            return amount;
        }

        /// <summary>
        ///     Ends the game if Earth ecosystem collapses or if Sentinel manages to preserve Earth during its shift.
        /// </summary>
        private void _handleEndGameScenarios()
        {
            if (resistance <= 0)
            {
                _gameManager.GameOver();
            }
            else
            {
                survivedTime += Time.deltaTime;
                if (survivedTime > ResistanceThreshold) _gameManager.GameBeaten();
            }
        }

        // Earth slowly regenerates itself.
        private void _handleRegeneration()
        {
            resistance = Mathf.Min(resistance + Time.deltaTime * 5, MAXResistance);
        }

        /// <summary>
        ///     Fire an alarm if the Earth ecosystem is threatened.
        /// </summary>
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