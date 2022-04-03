using UnityEngine;

namespace Ships
{
    /// <summary>
    ///     SpaceShip class controls basic behaviour of a space ship.
    ///     It controls special effects and taking damage upon collisions.
    /// </summary>
    public class SpaceShip : MonoBehaviour
    {
        public float energyLevel = 1000;
        public bool alive = true;
        private AudioSource _explosionAudio;
        private ParticleSystem _explosionDestroyVFX;
        private ParticleSystem _explosionSmokeVFX;
        private ParticleSystem _explosionVFX;
        private bool _hasShield;
        private GameObject _shield;
        protected float MAXEnergyLevel;

        protected virtual void Start()
        {
            MAXEnergyLevel = energyLevel;
            var parentTransform = transform.parent.transform;
            _explosionVFX = parentTransform.Find("Explosion").GetComponent<ParticleSystem>();
            _explosionAudio = parentTransform.Find("jukebox")?.GetComponent<AudioSource>();
            var explosionDestroy = parentTransform.Find("ExplosionDestroy");
            if (explosionDestroy is { }) _explosionDestroyVFX = explosionDestroy.GetComponent<ParticleSystem>();
            _explosionDestroyVFX = !(_explosionDestroyVFX is null) ? _explosionDestroyVFX : _explosionVFX;
            var explosionSmoke = parentTransform.Find("ExplosionSmoke");
            if (explosionSmoke is { }) _explosionSmokeVFX = explosionSmoke.GetComponent<ParticleSystem>();
            _explosionSmokeVFX = _explosionSmokeVFX ? _explosionSmokeVFX : _explosionVFX;
            var shield = transform.Find("Shield");
            if (shield is null) return;
            _shield = shield.gameObject;
            _hasShield = true;
        }

        private void Update()
        {
            if (_hasShield) _shield.SetActive(energyLevel >= 300);
        }

        /// <summary>
        ///     Check collisions against ships and space stations
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            CheckCollisionWithSpaceStation(other);
            CheckCollisionWithSpaceShip(other);
        }

        /// <summary>
        ///     Take damage upon collisions with space stations.
        /// </summary>
        /// <param name="other">game object that might be a space station</param>
        private void CheckCollisionWithSpaceStation(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("SpaceStation")) TakeDamage(100);
        }

        /// <summary>
        ///     Take damage upon collision with a space ship.
        /// </summary>
        /// <param name="other">a game object that might be a space ship</param>
        private void CheckCollisionWithSpaceShip(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ship") &&
                other.gameObject.layer != LayerMask.NameToLayer("Sentinel")) return;
            TakeDamage(200);
        }

        /// <summary>
        ///     Take damage and return the amount of damage actually taken.
        /// </summary>
        /// <param name="damage">The amount of damage one intends to cause</param>
        /// <returns>The amount of damage actually taken.</returns>
        public virtual float TakeDamage(float damage)
        {
            energyLevel -= damage;
            _explode();
            return energyLevel;
        }

        /// <summary>
        ///     Play the special effects resulting from a collision.
        /// </summary>
        private void _explode()
        {
            if (_explosionAudio is { }) _explosionAudio.Play();
            if (energyLevel <= 0)
            {
                alive = false;
                if (_explosionDestroyVFX is null)
                {
                    Debug.Log("No destroy explosion found");
                    return; // the ship may have been destroyed
                }

                _explosionDestroyVFX.Clear();
                _explosionDestroyVFX.Stop();
                _explosionSmokeVFX.Clear();
                _explosionSmokeVFX.Stop();
                _explosionDestroyVFX.Play();
                _explosionSmokeVFX.Play();
                transform.gameObject.SetActive(false);
                Invoke(nameof(SetInactive), 1.2f);
            }
            else
            {
                _explosionVFX.Clear();
                _explosionVFX.Stop();
                _explosionVFX.Play();
            }
        }

        /// <summary>
        ///     Makes a ship inactive.
        ///     An enemy ship that is inactive will be removed from the game.
        ///     If the Sentinel ship is destroyed, the game is over.
        /// </summary>
        private void SetInactive()
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}