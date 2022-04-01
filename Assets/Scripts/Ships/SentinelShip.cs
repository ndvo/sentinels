using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ships
{
    public class SentinelShip : SpaceShip
    {
        private GameManager _gameManager; 
        private Image _shieldPowerUI;
        public GameObject collisionHelp;
        
        protected override void Start()
        {
            _gameManager = GameObject.Find("/GameManager").GetComponent<GameManager>();
            var shieldObject = GameObject.Find("/Canvas/Sentinel/Shield/ShieldPower");
            if (shieldObject is {}) _shieldPowerUI = shieldObject.GetComponent<Image>();
            base.Start();
            if (PlaySession.isPractice)
            {
                MAXEnergyLevel *= 10;
                energyLevel *= 10;
            }
        }

        public override float TakeDamage(float damage)
        {
            if (PlaySession.isPractice && energyLevel == MAXEnergyLevel )
                _gameManager.ShowHelp(collisionHelp);
            var inflictedDamage = base.TakeDamage(damage);
            if (energyLevel <= 0) _gameManager.GameOver();
            _updateUi();
            return inflictedDamage;
        }

        public void RecoverEnergy(float energy)
        {
            energyLevel += Random.value * energy;
            _updateUi();
        }

        private void _updateUi()
        {
            if (_shieldPowerUI is {}) _shieldPowerUI.fillAmount = energyLevel / MAXEnergyLevel;
        }
    }
}
