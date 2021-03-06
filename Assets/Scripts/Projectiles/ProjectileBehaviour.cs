using UnityEngine;

namespace Projectiles
{
    /// <summary>
    ///     Controls the behaviour of the projectiles (mines) launched by enemies.
    /// </summary>
    public class ProjectileBehaviour : MonoBehaviour
    {
        public float duration;
        private bool _fadeOut;
        private Material _material;

        private void Start()
        {
            _material = transform.GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            duration -= Time.deltaTime;
            if (duration <= 0) _fadeOut = true;

            if (!_fadeOut) return;
            var oldColor = _material.color;
            var newColor = Mathf.Max(0, oldColor.a - 0.05f * Time.deltaTime);
            _material.color = new Color(oldColor.r, oldColor.g, oldColor.b, newColor);
            if (_material.color.a <= 0f) Destroy(this);
        }
    }
}