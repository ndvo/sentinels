using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sky
{

    public class EarthBehaviour : MonoBehaviour
    {
        public float resistance = 1000f;

        private Image _resistanceImage;

        private float _maxResistance = 1000f;

        private void Start()
        {
            _resistanceImage = GameObject.Find("/Canvas/Earth/MineralResources/Overlay").GetComponent<Image>();
        }

        private void Update()
        {
            // Earth slowly regenerates itself.
            resistance += Time.deltaTime;
            resistance = Mathf.Clamp(resistance, 0, _maxResistance);
            _updateUi();
        }

        public void TakeDamage(float amount)
        {
            Debug.Log($"Total damage amount {amount}");
            resistance -= amount;
            _updateUi();
            Debug.Log($"Resistance {resistance}");
        }

        private void _updateUi()
        {
            _resistanceImage.fillAmount = resistance / _maxResistance;
        }
    }
}