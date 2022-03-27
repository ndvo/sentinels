using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm;
using Ships;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Headquarters : MonoBehaviour
{
    private Func<Individual[], float[][]> _protonLegacyGA;

    private GeneticAlgorithm.GeneticAlgorithm _ga;

    private GameObject _shipsContainer;

    public GameObject[] shipPrefabs;
    public int generationAmount = 3;
    public int maxShips = 15;
    private Transform _ships;

    private TextMeshProUGUI _uiTextNumberOfEnemies;
    private TextMeshProUGUI _uiTextNumberOfInfectedStations;

    private List<EnemyShipStateMachine> _shipStates = new List<EnemyShipStateMachine>();

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
        _uiTextNumberOfEnemies = GameObject.Find("/Canvas/EnemyHeadquarters/enemies").GetComponent<TextMeshProUGUI>();
        _uiTextNumberOfInfectedStations = GameObject.Find("/Canvas/EnemyHeadquarters/stations").GetComponent<TextMeshProUGUI>();
        Debug.Log($"Looking for texts in ui {_uiTextNumberOfEnemies} {_uiTextNumberOfInfectedStations}");
    }

    void _setGeneticAlgorithm()
    {
        _ga ??= new GeneticAlgorithm.GeneticAlgorithm();
    }

    void Update()
    {
        if (Random.value < 0.006 && _ships.childCount < maxShips)
        {
            var currentGeneration = _shipsContainer.GetComponentsInChildren<ProtonLegacy>()
                .Where(e => e.gameObject.activeSelf);
            var currentGen = currentGeneration.Select(
                r => r.GetIndividual()
            ).ToArray();
            GeneticAlgorithm.GeneticAlgorithm.SetArbitraryAchievements(currentGen);
            var newGen = SpawnGeneration();
            foreach (var s in newGen)
            {
                var stateMachine = s.GetComponent<EnemyBehaviour>().StateMachine;
                if (stateMachine is {}) _shipStates.Add(stateMachine);
                var protonLegacyBehaviour = s.GetComponent<ProtonLegacy>();
                if (protonLegacyBehaviour is { }) protonLegacyBehaviour.level = 1 + (int) (Time.time / 60);
            }
        }
        _updateUi();
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
    private GameObject[] SpawnGeneration()
    {
        var currentGeneration = _shipsContainer.GetComponentsInChildren<ProtonLegacy>();
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

        //foreach (Transform ship in _shipsContainer.transform) Destroy(ship.gameObject);
        var result = new GameObject[genomes.Length];
        for (var i = 0; i < result.Length; i++)
        {
            var chosenPrefab = Random.Range(0, shipPrefabs.Length);
            var ship = Object.Instantiate(
                shipPrefabs[chosenPrefab],
                transform.position,
                transform.rotation,
                _shipsContainer.transform
            );
            ship.GetComponent<ProtonLegacy>().SetGenome(genomes[i]);
            result[i] = ship;
        }

        return result;
    }

    private void _updateUi()
    {
        _uiTextNumberOfEnemies.text = $"Enemies: {_ships.childCount}";
        _uiTextNumberOfInfectedStations.text = $"Infected Stations: {_shipStates.Count(i => i.GetState() == EnemyShipStates.AttackingEarth)}";
    }
}