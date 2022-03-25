using UnityEngine;
using UnityEngine.UI;

namespace Ships
{
    public class SentinelShip : SpaceShip
    {
        private Image _shieldPowerUI;
        
        protected override void Start()
        {
            var shieldObject = GameObject.Find("/Canvas/Sentinel/Shield/ShieldPower");
            if (shieldObject is {}) _shieldPowerUI = shieldObject.GetComponent<Image>();
            base.Start();
        }

        public override float TakeDamage(float damage)
        {
            var inflictedDamage = base.TakeDamage(damage);
            if (_shieldPowerUI is {}) _shieldPowerUI.fillAmount = energyLevel / MAXEnergyLevel;
            return inflictedDamage;
        }
    }
}
