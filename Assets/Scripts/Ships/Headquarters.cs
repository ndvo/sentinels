using System;
using System.Linq;
using GeneticAlgorithm;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Headquarters : MonoBehaviour
{
    private Func<Individual[], float[][]> _protonLegacyGA;

    private GeneticAlgorithm.GeneticAlgorithm _ga;

    private GameObject _shipsContainer;

    public GameObject[] shipPrefabs;
    private Transform _ships;

    // Start is called before the first frame update
    private void Awake()
    {
        _shipsContainer = GameObject.Find("Ships");
    }

    void Start()
    {
        _ga = new GeneticAlgorithm.GeneticAlgorithm();
        _protonLegacyGA = _ga.GAFactory(
            (achievements) => achievements.Sum(),
            (r) => GeneticAlgorithm.GeneticAlgorithm.SelectionRoulette(r, 0.0f),
            GeneticAlgorithm.GeneticAlgorithm.MatchingLeaderChoice,
            GeneticAlgorithm.GeneticAlgorithm.UniformCrossOver
        );
        _ships = GameObject.Find("Ships").transform;
    }

    void _setGeneticAlgorithm()
    {
        _ga ??= new GeneticAlgorithm.GeneticAlgorithm();
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value < 0.01f && _ships.childCount < 30)
        {
            var currentGeneration = _shipsContainer.GetComponentsInChildren<ProtonLegacy>();
            var currentGen = currentGeneration.Select(
                r => r.GetIndividual()
            ).ToArray();
            GeneticAlgorithm.GeneticAlgorithm.SetArbitraryAchievements(currentGen);
            SpawnGeneration();
        }
    }

    /// <summary>
    /// Creates a new ShipGenome from random values.
    /// </summary>
    /// <returns>The random ShipGenome</returns>
    public ShipGenome NewShipGenome()
    {
        _setGeneticAlgorithm();
        var randomShip = _ga.GenerateRandomShip();
        return new ShipGenome(randomShip);
    }

    /// <summary>
    /// Creates a generation of N random genomes of ships.
    /// </summary>
    /// <param name="n">number of different random genomes to create</param>
    /// <returns></returns>
    public ShipGenome[] NewShipGenomeGeneration(int n)
    {
        return (from s in new float[n] select NewShipGenome()).ToArray();
    }

    /// <summary>
    /// Spawns n new enemy ships for this HeadQuarters.
    ///
    /// If there is already an existing generation, it will use the genetic algorithm to evaluate it and breed the next
    /// generation from it.
    /// </summary>
    /// <param name="n"></param>
    /// <returns>The new generation of ships</returns>
    private GameObject[] SpawnGeneration(int n = 7)
    {
        var currentGeneration = _shipsContainer.GetComponentsInChildren<ProtonLegacy>();
        ShipGenome[] genomes;
        if (currentGeneration.Length == 0)
        {
            genomes = new ShipGenome[n];
            for (var i = 0; i < n; i++)
            {
                genomes[i] = NewShipGenome();
            }
        }
        else
        {
            var currentGen = currentGeneration.Select(
                r => r.GetIndividual()
            ).ToArray();
            genomes = _protonLegacyGA(currentGen).Select(r => new ShipGenome(r)).ToArray();
            for (var i = 0; i < currentGen.Length - genomes.Length; i++)
            {
                genomes = genomes.Append<ShipGenome>(NewShipGenome()).ToArray();
            }
        }

        foreach (Transform ship in _shipsContainer.transform) Destroy(ship.gameObject);
        var result = new GameObject[genomes.Length];
        for (var i = 0; i < result.Length; i++)
        {
            var ship = Object.Instantiate(
                shipPrefabs[Random.Range(0, shipPrefabs.Length - 1)],
                transform.position,
                transform.rotation,
                _shipsContainer.transform
            );
            ship.GetComponent<ProtonLegacy>().SetGenome(genomes[i]);
            result[i] = ship;
        }

        return result;
    }
}