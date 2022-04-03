using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm;
using UnityEngine;
using Utils;
using Time = UnityEngine.Time;

namespace Ships
{
    /// <summary>
    ///     ProtonLegacy class is responsible for converting the genome into a ProtonLegacy ship.
    ///     The name Proton Legacy is the name of the third party model that is used to create enemies in the game.
    ///     This class provides the transformations from the genome values (an array of floats) into ship's model and
    ///     behaviour.
    /// </summary>
    public class ProtonLegacy : MonoBehaviour
    {
        public bool ready;
        public float[] achievements;
        public Transform[] bodies;
        public Transform[] bridges;
        public Transform[] laserCannons;
        public Transform[] missileLaunchers;
        public Transform[] tractors;
        public Transform[] turbines;
        public Transform[] wings;
        public float level = 1;
        private AttackEarth _attackEarth;
        private AttackSentinel _attackSentinel;
        private EnemyBehaviour _behaviour;
        private Transform _core;
        private SimpleFlight _flight;
        private ShipGenome _genome;
        private Headquarters _headquarters;
        private GameObject _headquartersGameObject;
        private Individual _individual;

        private void Start()
        {
            _core = transform.Find("Core");
            _headquartersGameObject = GameObject.Find("EnemyHeadQuarters");
            if (_headquartersGameObject != null)
                _headquarters = _headquartersGameObject.GetComponent<Headquarters>();
            _flight = GetComponent<SimpleFlight>();
            _behaviour = GetComponent<EnemyBehaviour>();
            _attackEarth = GetComponent<AttackEarth>();
            _attackSentinel = GetComponent<AttackSentinel>();
            _IdentifyParts();
            _ApplyGenome();
            _SetFeatures();
        }

        private void Update()
        {
            _updateAchievements();
        }

        /// <summary>
        ///     Makes the provide ShipGenome the genome of this ship.
        /// </summary>
        /// <param name="genome"></param>
        public void SetGenome(ShipGenome genome)
        {
            _genome = genome;
            _individual = new Individual
            {
                Genes = _genome.GetGenome(),
                Achievements = new float[2] // the first item measures, time alive
            };
        }

        /// <summary>
        ///     Find the GameObject parts that make for this ship.
        ///     For new models to be used (aside ProtonLegacy) it is necessary to rethink these parts.
        /// </summary>
        private void _IdentifyParts()
        {
            bodies = GetParts("ShipBody");
            bridges = GetParts("ShipBridge");
            laserCannons = GetParts("ShipLaserCannon");
            missileLaunchers = GetParts("ShipMissileLauncher");
            tractors = GetParts("ShipTractor");
            turbines = GetParts("ShipTurbines");
            wings = GetParts("ShipWings");
        }

        /// <summary>
        ///     Helper function to find all the available parts.
        /// </summary>
        /// <returns>An iterable with the child parts.</returns>
        private IEnumerable<Transform> _parts()
        {
            for (var i = 0; i < transform.childCount; i++) yield return _core.GetChild(i);
        }

        /// <summary>
        ///     Get an array of transforms that contain the tag partType.
        /// </summary>
        /// <param name="partType"></param>
        /// <returns>The array of transforms with the given tag.</returns>
        public Transform[] GetParts(string partType)
        {
            return _parts()
                .Where(r => r.CompareTag(partType))
                .ToArray();
        }

        /// <summary>
        ///     Set current achievements
        ///     So far there are only two metrics of success:
        ///     - damage inflicted upon Earth
        ///     - time alive
        ///     TODO: new metric: damage inflicted upon Sentinel
        /// </summary>
        private void _updateAchievements()
        {
            if (_attackEarth)
            {
                _individual.Achievements[0] += _attackEarth.inflicted;
                _attackEarth.inflicted = 0;
            }

            if (gameObject.activeSelf)
                _individual.Achievements[1] += Time.deltaTime;
        }

        /// <summary>
        ///     Transforms the ship in accordance with the genome.
        /// </summary>
        private void _ApplyGenome()
        {
            if (_genome == null) return;
            // this helper structure simplifies the application of the genome
            // for each type of transformable ship part we provide the corresponding genes.
            var GenomePartPair = new[]
            {
                new {parts = bodies, genes = _genome.Body},
                new {parts = bridges, genes = _genome.Bridge},
                new {parts = laserCannons, genes = _genome.LaserCannon},
                new {parts = missileLaunchers, genes = _genome.MissileLauncher},
                new {parts = tractors, genes = _genome.Tractor},
                new {parts = wings, genes = _genome.Wing}
            };

            static void TranslateOnZ(Transform b, float v)
            {
                b.Translate(new Vector3(0f, 0f, v), Space.Self);
            }

            static Action<Transform> RotateOnY(float v)
            {
                return b => b.Rotate(new Vector3(0f, v, 0f), Space.Self);
            }

            foreach (var part in GenomePartPair)
            {
                // position
                foreach (var r in part.parts) TranslateOnZ(r, part.genes.position);
                // rotation
                var rotation = part.genes.rotation;
                Iterables.SymmetricalApply(
                    turbines, RotateOnY(rotation), RotateOnY(-1 * rotation));
                // size
                foreach (var r in part.parts) r.localScale *= part.genes.size;
            }
        }

        /// <summary>
        ///     Transforms the genetic features into behaviour features.
        ///     It maps the values from the genome code (between 0 and 1) into meaningful values for the ProtonLegacy family of
        ///     enemies.
        /// </summary>
        private void _SetFeatures()
        {
            _behaviour.resistance = (10 + 5 * level) * _genome.Resistance;
            _behaviour.firePower = 10 * level * _genome.FirePower;
            _behaviour.drainPower = 5 * level * _genome.DrainPower;
            _flight.speed = 2 + level * _genome.MovementSpeed;
            _behaviour.fleeTime = 10 * level * _genome.FleeTime;
            _behaviour.attackProbability = Mathf.Clamp(80 * level * _genome.AttackProbability, 0, 100);
            _behaviour.idleProbability = Mathf.Clamp((20 - level) * _genome.IdleProbability, 0, 60);
            _behaviour.sentinelSensorSize = 30 + 80 * _genome.AttackSensorSize;
            _behaviour.navigationSensorSize = 15 + 20 * _genome.NavigationSensorSize;
        }

        public Individual GetIndividual()
        {
            return _individual;
        }
    }
}