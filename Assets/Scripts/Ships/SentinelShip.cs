using System;
using Sky;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Ships
{
    /// <summary>
    ///     Sentinel Ship
    ///     Sentinel Ship behaviour handles taking damage and recovering energy drained from other vessels.
    /// </summary>
    public class SentinelShip : SpaceShip
    {
        public GameObject collisionHelp;
        private EarthBehaviour _earth;
        private GameManager _gameManager;
        private Image _shieldPowerUI;

        protected override void Start()
        {
            _gameManager = GameObject.Find("/GameManager").GetComponent<GameManager>();
            _earth = GameObject.Find("/Earth").GetComponent<EarthBehaviour>();
            var shieldObject = GameObject.Find("/Canvas/Sentinel/Shield/ShieldPower");
            if (shieldObject is { }) _shieldPowerUI = shieldObject.GetComponent<Image>();
            base.Start();
            if (PlaySession.IsPractice)
            {
                MAXEnergyLevel *= 10;
                energyLevel *= 10;
            }
        }

        /// <summary>
        ///     Compute and apply damage.
        ///     This function is set to public so that it can be called by enemy objects.
        ///     Even though this basic allows enemy objects to decide on the damage they cause, it also allow the requested
        ///     damage to be processed here.
        /// </summary>
        /// <param name="damage">the amount of damage one expect to inflict.</param>
        /// <returns>the amount of damage that was actually inflicted.</returns>
        public override float TakeDamage(float damage)
        {
            if (PlaySession.IsPractice && Math.Abs(energyLevel - MAXEnergyLevel) < 0.1)
                _gameManager.ShowHelp(collisionHelp);
            var inflictedDamage = base.TakeDamage(damage);
            if (energyLevel <= 0) _gameManager.GameOver();
            _updateUi();
            return inflictedDamage;
        }

        /// <summary>
        ///     Recover part of the energy taken from enemy
        ///     It recovers at least 40% of the energy drained from the enemy vessel.
        ///     The energy recovered is divided between Earth and the Sentinel.
        ///     The less energy Earth needs, the more energy Sentinel can keep for itself.
        /// </summary>
        /// <param name="energy"></param>
        public void RecoverEnergy(float energy)
        {
            var efficiency = Mathf.Clamp(Random.value, 0.4f, 1);
            var recovered = energy * efficiency;
            var earthShare = 1 - _earth.resistance / _earth.maxResistance;
            energyLevel += (1 - earthShare) * recovered;
            _earth.resistance += earthShare * recovered;
            _updateUi();
        }

        private void _updateUi()
        {
            if (_shieldPowerUI is { }) _shieldPowerUI.fillAmount = energyLevel / MAXEnergyLevel;
        }
    }
}