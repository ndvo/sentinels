using UnityEngine;
using UnityEngine.UI;

namespace Ships
{
    public class SentinelShip : SpaceShip
    {
        private Image _shieldPowerUI;
        
        protected override void Start()
        {
            _shieldPowerUI = GameObject.Find("/Canvas/Sentinel/Shield/ShieldPower").GetComponent<Image>();
            base.Start();
        }

        public override float TakeDamage(float damage)
        {
            var inflictedDamage = base.TakeDamage(damage);
            _shieldPowerUI.fillAmount = energyLevel / MAXEnergyLevel;
            return inflictedDamage;
        }
    }
}
