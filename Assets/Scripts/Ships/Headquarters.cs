using System.Linq;
using GeneticAlgorithm;
using UnityEngine;

public class Headquarters : MonoBehaviour
{
    private GeneticAlgorithm.GeneticAlgorithm _ga;
    // Start is called before the first frame update
    void Start()
    {
    }

    void _setGeneticAlgorithm()
    {
        _ga = this._ga == null 
            ? new GeneticAlgorithm.GeneticAlgorithm()
            : _ga;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
    
}
