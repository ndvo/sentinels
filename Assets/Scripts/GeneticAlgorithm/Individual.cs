namespace GeneticAlgorithm
{
    /// <summary>
    ///     An individual a data structure that contains the genes to create a ship and the achievements recorded for such
    ///     ship.
    /// </summary>
    public class Individual
    {
        public float[] achievements;
        public float fitness;
        public float[] genes;
    }
}