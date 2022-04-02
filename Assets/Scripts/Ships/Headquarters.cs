using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm;
using Ships;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

/// <summary>
/// The enemy headquarters is responsible for instantiating new enemy ships.
///
/// The headquarters itself fly off board. This should make the distribution of entry places for enemy ships more
/// realistic.
/// </summary>
public class Headquarters : MonoBehaviour
{
    private Func<Individual[], float[][]> _protonLegacyGA;

    private GeneticAlgorithm.GeneticAlgorithm _ga;

    public GameObject[] shipPrefabs;
    public int generationAmount = 3;
    public int maxShips = 15;
    private GameObject _ships;

    private TextMeshProUGUI _uiTextNumberOfEnemies;
    private TextMeshProUGUI _uiTextNumberOfInfectedStations;

    private List<EnemyShipStateMachine> _shipStates = new List<EnemyShipStateMachine>();

    private GameManager _gameManager;

    void Start()
    {
        _ga = new GeneticAlgorithm.GeneticAlgorithm();
        // Create the Genetic Algorithm factory that will generate the new ship's genomes
        _protonLegacyGA = _ga.GAFactory(
            (achievements) => achievements.Sum(),
            (r) => GeneticAlgorithm.GeneticAlgorithm.SelectionRoulette(r, 0.0f),
            GeneticAlgorithm.GeneticAlgorithm.MatchingLeaderChoice,
            GeneticAlgorithm.GeneticAlgorithm.UniformCrossOver
        );
        _ships = GameObject.Find("Ships");
        _uiTextNumberOfEnemies = GameObject.Find("/Canvas/EnemyHeadquarters/enemies").GetComponent<TextMeshProUGUI>();
        _uiTextNumberOfInfectedStations =
            GameObject.Find("/Canvas/EnemyHeadquarters/stations").GetComponent<TextMeshProUGUI>();
        _gameManager = GameObject.Find("/GameManager").GetComponent<GameManager>();
    }

    private void _setGeneticAlgorithm()
    {
        _ga ??= new GeneticAlgorithm.GeneticAlgorithm();
    }

    void Update()
    {
        if (_gameManager.Paused) return;
        var childCount = _ships.transform.childCount;
        if (Random.value < 0.005 && childCount < maxShips)
        {
            var newGen = SpawnGeneration();
            foreach (var s in newGen)
            {
                _storeShipStateMachine(s);
                _setShipLevel(s);
            }
        }
        _updateUi();
    }

    /// <summary>
    /// Store created ships statemachines to be able ot act on them depending on the state
    /// </summary>
    /// <param name="ship"></param>
    private void _storeShipStateMachine(GameObject ship)
    {
        var stateMachine = ship.GetComponent<EnemyBehaviour>().StateMachine;
        if (stateMachine is { }) _shipStates.Add(stateMachine);
    }
    
    /// <summary>
    /// Determine the level of the ships.
    /// </summary>
    /// <param name="ship"></param>
    private void _setShipLevel(GameObject ship)
    {
        var protonLegacyBehaviour = ship.GetComponent<ProtonLegacy>();
        if (protonLegacyBehaviour is { }) protonLegacyBehaviour.level = 1 + (int) (Time.time / 60);
    }

    /// <summary>
    /// Creates a new ShipGenome from random values.
    /// </summary>
    /// <returns>The random ShipGenome</returns>
    public ShipGenome NewShipGenome()
    {
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
    private GameObject[] SpawnGeneration()
    {
        var currentGeneration = _ships.GetComponentsInChildren<ProtonLegacy>()
            .Where(e => e.gameObject.activeSelf).ToArray();
        ShipGenome[] genomes;
        if (currentGeneration.Length == 0)
        {
            genomes = new ShipGenome[generationAmount];
            for (var i = 0; i < generationAmount; i++)
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
        var result = new GameObject[genomes.Length];
        for (var i = 0; i < result.Length; i++)
        {
            var chosenPrefab = Random.Range(0, shipPrefabs.Length);
            var ship = Object.Instantiate(
                shipPrefabs[chosenPrefab],
                transform.position,
                transform.rotation,
                _ships.transform
            );
            ship.GetComponent<ProtonLegacy>().SetGenome(genomes[i]);
            result[i] = ship;
        }
        return result;
    }

    private void _updateUi()
    {
        _uiTextNumberOfEnemies.text = $"Enemies: {_ships.transform.childCount}";
        _uiTextNumberOfInfectedStations.text = $"Infected Stations: {_shipStates.Count(i => i.GetState() == EnemyShipStates.AttackingEarth)}";
    }
}