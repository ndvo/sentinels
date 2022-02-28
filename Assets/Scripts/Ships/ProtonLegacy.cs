using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm;
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
    
    // Start is called before the first frame update
    void Start()
    {
        _headquartersGameObject = GameObject.Find("EnemyHeadQuarters");
        if (_headquartersGameObject != null)
            _headquarters = _headquartersGameObject.GetComponent<Headquarters>();
        _IdentifyParts();
    }
    
    public void SetGenome(ShipGenome genome)
    {
        _genome = genome;
        _IdentifyParts();
        _ApplyGenome();
        _individual = new Individual
        {
            genes = _genome.GetGenome(),
            achievements = new float[7]
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

    // Update is called once per frame
    void Update()
    {
        if (_genome != null)
        {
        }
    }

    private void _ApplyGenome()
    {
        if (_genome == null) return;
        var GenomePartPair = new[]
        {
            new {parts = bodies, genes = _genome.body},
            new {parts = bridges, genes = _genome.bridge},
            new {parts = laserCannons, genes = _genome.laserCannon},
            new {parts = missileLaunchers, genes = _genome.missileLauncher},
            new {parts = tractors, genes = _genome.tractor},
            new {parts = wings, genes = _genome.wing},
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

    public Individual GetIndividual()
    {
        return _individual;
    }

}
