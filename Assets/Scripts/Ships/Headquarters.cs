using System.Collections;
using System.Collections.Generic;
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
}
