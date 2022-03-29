using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm;
using Ships;
using UnityEngine;

public class ProtonLegacy : MonoBehaviour
{
    public bool ready = false;
    private GameObject _headquartersGameObject;
    private Headquarters _headquarters;
    private ShipGenome _genome;
    private Individual _individual;
    public float[] achievements;
    public Transform[] bodies;
    public Transform[] bridges;
    public Transform[] laserCannons;
    public Transform[] missileLaunchers;
    public Transform[] tractors;
    public Transform[] turbines;
    public Transform[] wings;
    private SimpleFlight _flight;
    private EnemyBehaviour _behaviour;
    private AttackEarth _attackEarth;
    public float level = 1;
    
    void Start()
    {
        _headquartersGameObject = GameObject.Find("EnemyHeadQuarters");
        if (_headquartersGameObject != null)
            _headquarters = _headquartersGameObject.GetComponent<Headquarters>();
        _flight = GetComponent<SimpleFlight>();
        _behaviour = GetComponent<EnemyBehaviour>();
        _attackEarth = GetComponent<AttackEarth>();
        _IdentifyParts();
        _ApplyGenome();
        _SetFeatures();
    }
    
    public void SetGenome(ShipGenome genome)
    {
        _genome = genome;
        _individual = new Individual
        {
            genes = _genome.GetGenome(),
            achievements = new float[2] // the first item measures, time alive
        };
    }

    void _IdentifyParts()
    {
        bodies = GetParts("ShipBody");
        bridges = GetParts("ShipBridge");
        laserCannons = GetParts("ShipLaserCannon");
        missileLaunchers = GetParts("ShipMissileLauncher");
        tractors = GetParts("ShipTractor");
        turbines = GetParts("ShipTurbines");
        wings = GetParts("ShipWings");
    }

    private IEnumerable<Transform> _parts()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            yield return transform.GetChild(i);
        }
    }

    public Transform[] GetParts(string partType)
    {
        return _parts()
            .Where(r => r.CompareTag(partType))
            .ToArray();
    }

    void Update()
    {
        _updateAchievements();
    }

    private void _updateAchievements()
    {
        if (_attackEarth)
        {
            _individual.achievements[0] += _attackEarth.inflicted;
            _attackEarth.inflicted = 0;
        }
        if (gameObject.activeSelf)
            _individual.achievements[1] += Time.deltaTime;
    }

    private void _ApplyGenome()
    {
        if (_genome == null) return;
        var GenomePartPair = new[]
        {
            new {parts = bodies, genes = _genome.Body},
            new {parts = bridges, genes = _genome.Bridge},
            new {parts = laserCannons, genes = _genome.LaserCannon},
            new {parts = missileLaunchers, genes = _genome.MissileLauncher},
            new {parts = tractors, genes = _genome.Tractor},
            new {parts = wings, genes = _genome.Wing},
        };
        static void TranslateOnZ(Transform b, float v) =>
            b.Translate(new Vector3(0f, 0f, v), Space.Self);
        static Action<Transform> RotateOnY(float v) =>
            (Transform b) => b.Rotate(new Vector3(0f, v, 0f), Space.Self);
        foreach (var part in GenomePartPair)
        {
            // position
            foreach (var r in part.parts) TranslateOnZ(r, part.genes.position);
            // rotation
            var rotation = part.genes.rotation;
            Utils.Iterables.SymmetricalApply(
                turbines, RotateOnY(rotation), RotateOnY(-1 * rotation));
            // size
            foreach (var r in part.parts) r.localScale *= part.genes.size;
        }
    }

    /// <summary>
    /// Transforms the genetic features into behaviour features.
    ///
    /// It maps the values from the genome code (between 0 and 1) into meaningful values for the ProtonLegacy family of
    /// enemies.
    /// </summary>
    private void _SetFeatures()
    {
        _behaviour.resistance = (10 + 5 * level) * _genome.Resistance;
        _behaviour.firePower = (10 * level) * _genome.FirePower;
        _behaviour.drainPower = (10 * level) * _genome.DrainPower;
        _behaviour.movementSpeed = 2 + level/2 * _genome.MovementSpeed;
        _behaviour.fleeTime = (10 * level) * _genome.FleeTime;
        _behaviour.attackProbability = Mathf.Clamp((80 * level) * _genome.AttackProbability, 0, 100);
        _behaviour.idleProbability = Mathf.Clamp((20 - level) * _genome.IdleProbability, 0, 60);
    }

    public Individual GetIndividual()
    {
        return _individual;
    }

}
