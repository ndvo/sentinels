using System;
using System.Linq;
using Random = System.Random;

namespace GeneticAlgorithm
{
    
/// <summary>
/// The GeneticAlgorithm class manages the Execution of the genetic algorithm.
///
/// The idea is that a solution for a problem is encoded and evolved over generations starting from a random starting
/// point.
///
/// The problem, in our case, is finding an enemy ship, mine or missile that best achieves its goals.
/// The solution is encoded as an array of floating point numbers.
/// Each number is a gene and can be decoded into a feature.
/// Each array of genes is a genome and can be decoded into a full solution.
/// An individual is a solution with a record of achievements (a solution that has already performed and can be
/// evaluated).
/// A generation is a set of genomes.
/// 
/// </summary>
public class GeneticAlgorithm 
{

    public float[] GenerateRandomShip()
    {
        var random = new Random(Utils.Time.UnixNow());
        var genome = new float[35];
        for (var i=0; i<35; i++)
        {
            genome[i] = (float) random.NextDouble();
        }
        return genome;
    }

    /// <summary>
    /// Applies a fitness function to each individual ard return a sorted array of fittest.
    /// </summary>
    /// <param name="generation"></param>
    /// <param name="fitness"></param>
    /// <returns></returns>
    public static Individual[] EvaluateGeneration(Individual[] generation, Func<float[], float> fitness)
    {
        foreach (var ind in generation) ind.fitness = fitness(ind.achievements);
        return generation.OrderBy(r => r.fitness).ToArray();
    }

    /// <summary>
    /// Selection algorithms receive as input a Generation (array of individuals) and output an array of surviving
    /// Individuals.
    /// 
    /// Roulette selection accomplishes this by giving individuals a change of being selected as high as their
    /// proportional fitness value.
    ///
    /// Please, notice that the Roulette Selection makes it likely for the same individual to be over represented in
    /// the next generation. Each draw from the pool takes into consideration the initial probabilities. Nothing else.
    /// </summary>
    /// <param name="generation">The generation to be selected from. The generation must have been already evaluated.
    /// </param>
    /// <param name="deathRate">The proportion of the individuals that are not going to be selected.</param>
    /// <returns></returns>
    public static Individual[] SelectionRoulette(Individual[] generation, float deathRate = 0.2f)
    {
        var fitnessSum = generation.Sum(r => r.fitness);
        var ranking = new float[generation.Length];
        var previous = 0f;
        for (var i = 0; i < generation.Length; i++)
        {
            ranking[i] = (generation[i].fitness / fitnessSum) + previous;
            previous = ranking[i];
        }
        var selectionSize = (int) (generation.Length * (1 - deathRate));
        var selected = new Individual[selectionSize];
        var random = new Random(Utils.Time.UnixNow());
        for (int i = 0; i < (int) selectionSize; i++)
        {
            var chosenValue = random.NextDouble();
            for (int ii = 0; ii < generation.Length; ii++)
            {
                if (chosenValue > ranking[ii]) continue;
                var value = ii <= 0 ? 0 : ii - 1;
                selected[i] = generation[value];
                break;
            }
        }
        return selected;
    }

    public static float[] OnePointCrossOver(float[] genomeA, float[] genomeB)
    {
        return KPointCrossOver(genomeA, genomeB, 1);
    }

    public static float[] KPointCrossOver(float[] genomeA, float[] genomeB, int k)
    {
        var result = new float[genomeA.Length];
        var random = new Random(Utils.Time.UnixNow());
        var kPoints = new int[k];
        for (var i = 0; i < k; i++) kPoints[i] = random.Next(0, genomeA.Length);
        Array.Sort(kPoints);
        var coin = 0;
        for (var i = 0; i < genomeA.Length; i++)
        {
            if (Array.IndexOf(kPoints, i) != -1) coin = (coin + 1) % 2;
            var gene = coin == 1 ? genomeB[i] : genomeA[i];
            result[i] = gene;
        }
        return result;
    }
    
    public static float[] UniformCrossOver(float[] genomeA, float[] genomeB)
    {
        var result = new float[genomeA.Length];
        var random = new Random(Utils.Time.UnixNow());
        for (var i = 0; i < genomeA.Length; i++)
        {
            var coin = random.Next(1, 3);
            var gene = coin == 1 ? genomeA[i] : genomeB[i];
            result[i] = gene;
        }
        return result;
    }

    /// <summary>
    /// Mutates with given probability each of the genes from the genome.
    ///
    /// Mutation occur randomly. For each gene a random number between 0 and 1 is chosen and if it is greater than
    /// mutationProbability the gene is mutated.
    /// Each mutation is capped to an increment of 0.02, but its value is random. The increment cap can be configured
    /// by setting mutationIncrement.
    /// </summary>
    /// <param name="genome">The genome to be mutated.</param>
    /// <param name="mutationProbability">The probability of each mutation.</param>
    /// <param name="mutationIncrement">The maximum increment (or decrement) of each mutation.</param>
    /// <returns></returns>
    public static float[] Mutation(
        float[] genome,
        float mutationProbability = 0.01f,
        float mutationIncrement = 0.02f
        )
    {
        var result = new float[genome.Length];
        var random = new Random(Utils.Time.UnixNow());
        for (int i = 0; i < genome.Length; i++)
        {
            var increment = 0f;
            if (random.NextDouble() < mutationProbability)
            {
                var decrementModifier = random.NextDouble() >= 0.5 ? -1 : 1;
                increment = (float) random.NextDouble() * mutationIncrement * decrementModifier;
            }
            result[i] = genome[i] + increment;
        }
        return result;
    }

}

public class Individual
{
    public float[] achievements;
    public float[] genes;
    public float fitness;
}

}
