using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace GeneticAlgorithm
{

    /// <summary>
    /// ShipGenome is the complete genome of a full ship.
    ///
    /// It aggregates in a semantic manner all of the genomes for each of the ship's parts.
    /// It also maps the information on the genome into ship features.
    ///
    /// For example, each 3rd gene may be responsible for determine the speed, or the higher value gene may responsible
    /// for strength. These are hypothetical examples. Refer to the code to understand the current actual mapping.
    /// </summary>
    public class ShipGenome
    {
        private float[] _data;
        public readonly ShipPartGenome Body;
        public readonly ShipPartGenome Bridge;
        public readonly ShipPartGenome LaserCannon;
        public readonly ShipPartGenome MissileLauncher;
        public readonly ShipPartGenome Tractor;
        public readonly ShipPartGenome Turbine;
        public readonly ShipPartGenome Wing;
        public readonly float Resistance;
        public readonly float MovementSpeed;
        public readonly float AttackProbability;
        public readonly float IdleProbability;
        public readonly float FirePower;
        public readonly float DrainPower;
        public readonly float FleeTime;
        
        public ShipGenome (float[] data)
        {
            _data = data;
            // Set the body parts
            Body = new ShipPartGenome(data.SubArray(0, 5));
            Bridge = new ShipPartGenome(data.SubArray(5, 5));
            LaserCannon = new ShipPartGenome(data.SubArray(10, 5));
            MissileLauncher = new ShipPartGenome(data.SubArray(15, 5));
            Tractor = new ShipPartGenome(data.SubArray(20, 5));
            Turbine = new ShipPartGenome(data.SubArray(25, 5));
            Wing = new ShipPartGenome(data.SubArray(30, 5));
            // Set the features
            // - All values are set in such a way that it is best to have the highest value
            // - All values are represented between 0 and 1
            Resistance = _data.Where((x, i) => i % 5 == 0).Aggregate((acc, i) => acc + Mathf.Abs(i)) / _data.Length / 5; // sizes
            FirePower = _std(_data) / _std(_data.Select((x, i) => i % 2 == 0 ? 1f : 0f)); // normalized std deviation
            DrainPower = _data.Average(); // how close it is close to 1
            MovementSpeed = _data.SubArray(25, 10).Sum() / 10f; // average of turbine and wings
            FleeTime = _data.Max(); // the maximum value across all
            AttackProbability = _data.Select(x => 1 - Mathf.Abs(x - 0.5f)).Sum() / _data.Length; // how close it is close to 0.5
            IdleProbability = 1f - _data.Min(); // the minimum value across all
        }

        /// <summary>
        /// Convert from Genome to Array.
        /// </summary>
        /// <returns>The ship's genome.</returns>
        public float[] GetGenome()
        {
            return Body.GetGene()
                .Concat(Bridge.GetGene())
                .Concat(LaserCannon.GetGene())
                .Concat(MissileLauncher.GetGene())
                .Concat(Tractor.GetGene())
                .Concat(Turbine.GetGene())
                .Concat(Wing.GetGene()).ToArray();
        }

        /// <summary>
        /// Helper function that returns the Standard Deviation for a list.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The standard deviation</returns>
        private static float _std(IEnumerable<float> data)
        {
            var enumerable = data as float[] ?? data.ToArray();
            return (float) Math.Sqrt(enumerable.Average(x=>x*x) - Math.Pow(enumerable.Average(),2f));
        }
    }

}