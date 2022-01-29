using System.Linq;
using Utils;

namespace GeneticAlgorithm
{

    /// <summary>
    /// ShipGenome is the complete genome of a full ship.
    ///
    /// It aggregates in a semantic manner all of the genomes for each of the ship's parts.
    ///
    /// </summary>
    public class ShipGenome
    {
        public readonly ShipPartGenome body;
        public readonly ShipPartGenome bridge;
        public readonly ShipPartGenome laserCannon;
        public readonly ShipPartGenome missileLauncher;
        public readonly ShipPartGenome tractor;
        public readonly ShipPartGenome turbine;
        public readonly ShipPartGenome wing;
        
        public ShipGenome (float[] data)
        {
            body = new ShipPartGenome(data.SubArray(0, 5));
            bridge = new ShipPartGenome(data.SubArray(5, 5));
            laserCannon = new ShipPartGenome(data.SubArray(10, 5));
            missileLauncher = new ShipPartGenome(data.SubArray(15, 5));
            tractor = new ShipPartGenome(data.SubArray(20, 5));
            turbine = new ShipPartGenome(data.SubArray(25, 5));
            wing = new ShipPartGenome(data.SubArray(30, 5));
        }

        /// <summary>
        /// Convert from Genome to Array.
        /// </summary>
        /// <returns></returns>
        public float[] GetGenome()
        {
            return body.GetGene()
                .Concat(bridge.GetGene())
                .Concat(laserCannon.GetGene())
                .Concat(missileLauncher.GetGene())
                .Concat(tractor.GetGene())
                .Concat(turbine.GetGene())
                .Concat(wing.GetGene()).ToArray();
        }
    }

}