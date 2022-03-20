using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sky
{
    public enum NaturalResources
    {
        Mineral,
        Biological,
        Atmospherical
    }

    public class EarthBehaviour : MonoBehaviour
    {
        public float mineralResources = 1000f;
        public float biologicalResources = 1000f;
        public float atmosphericalResources = 1000f;

        private Image _mineralResourcesImage;
        private Image _biologicalResourcesImage;
        private Image _atmosphericalResourcesImage;

        private float _maxResource = 1000f;

        private void Start()
        {
            _mineralResourcesImage = GameObject.Find("/Canvas/Earth/MineralResources/Overlay").GetComponent<Image>();
            _biologicalResourcesImage =
                GameObject.Find("/Canvas/Earth/BiologicalResources/Overlay").GetComponent<Image>();
            _atmosphericalResourcesImage =
                GameObject.Find("/Canvas/Earth/AtmosphericalResources/Overlay").GetComponent<Image>();
        }

        private void Update()
        {
            // Earth slowly regenerates itself.
            mineralResources += Mathf.Min(Time.deltaTime * 0.5f, _maxResource);
            biologicalResources += Mathf.Min(Time.deltaTime * 0.5f, _maxResource);
            atmosphericalResources += Mathf.Min(Time.deltaTime * 0.5f, _maxResource);
            _updateUi();
        }

        public void LooseResources(float amount, NaturalResources resource)
        {
            switch (resource)
            {
                case NaturalResources.Atmospherical:
                    atmosphericalResources -= amount;
                    break;
                case NaturalResources.Biological:
                    biologicalResources -= amount;
                    break;
                case NaturalResources.Mineral:
                    mineralResources -= amount;
                    break;
            }
        }

        private void _updateUi()
        {
            _atmosphericalResourcesImage.fillAmount = atmosphericalResources / _maxResource;
            _mineralResourcesImage.fillAmount = mineralResources / _maxResource;
            _biologicalResourcesImage.fillAmount = biologicalResources / _maxResource;
        }
    }
}