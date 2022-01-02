using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

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
        var random = new Random(Time.UnixNow());
        var genome = new float[35];
        for (var i=0; i<35; i++)
        {
            genome[i] = (float) random.NextDouble();
        }
        return genome;
    }

    public Func<Individual[], float[][]> GAFactory(
        Func<float[], float> fitnessFunction,
        Func<Individual[], Individual[]> selectionFunction,
        Func<Individual[], Func<float[], float>, Individual[][]> matchingFunction,
        Func<float[], float[], float[]> crossoverFunction 
    )
    {
        return (individuals)=> NewGeneration(
                individuals,
                fitnessFunction, selectionFunction, matchingFunction, crossoverFunction);
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
        return generation.OrderByDescending(r => r.fitness).ToArray();
    }

    /// <summary>
    /// Creates a new generation of individuals.
    ///
    /// This function implements the Genetic Algorithm pipeline.
    /// 1- Applies a fitness function to the given generation (an array of Individuals)
    /// 2- Applies a selection function to the given generation (create a smaller array of Individuals: the survivors)
    /// 3- Applies a matching function to the survivors (create an array of matches)
    /// 4- Applies a crossover function to the matches (create an array of genomes)
    /// </summary>
    /// <param name="previousGeneration">
    /// An array of individuals with both genes and achievements.
    /// </param>
    /// <param name="fitnessFunction">
    /// The fitness function takes the individual's achievements and return a fitness value.
    /// </param>
    /// <param name="selectionFunction">
    /// The selection function removes some individuals from the generation before the matching process.
    /// </param>
    /// <param name="matchingFunction">
    /// The matching function needs to receive a generation and return couples.
    /// The fitness function is passed into it so that it can be used to choose the pairs.
    /// </param>
    /// <param name="crossoverFunction"></param>
    public static float[][] NewGeneration(
        Individual[] previousGeneration,
        Func<float[], float> fitnessFunction,
        Func<Individual[], Individual[]> selectionFunction,
        Func<Individual[], Func<float[], float>, Individual[][]> matchingFunction,
        Func<float[], float[], float[]> crossoverFunction 
        )
    { 
        /* 1- Assess the generation
             in this step we apply a fitness function to the generation in order to fill each individual with the
             fitness result.
        */
        var _ = previousGeneration.Select(i => i.fitness = fitnessFunction(i.achievements));
        /* 2- Select the fittest (i.e. remove a percentage from the surviving pool)
            For this step we need a Selection Method and a Fitness Function
        */
        var survivors = selectionFunction(previousGeneration);
        /* 3- Create the matches
            We now match surviving individuals
         */
        var matches = matchingFunction(survivors, fitnessFunction);
        /* 4- Generate the offspring */
        var offspring = _breedMatches(
            matches.Select(r => new[] {r[0].genes, r[1].genes}).ToArray(),
            crossoverFunction);
        /* 5- Mutate:  
        */
        return offspring.Select(r => Mutation(r)).ToArray();
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
        var random = new Random(Time.UnixNow());
        for (int i = 0; i < selectionSize; i++)
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

    /// <summary>
    /// Create couples with complementary features following the leader's choice.
    ///
    /// Take the first ranked Individual and chooses a couple with the highest 
    /// </summary>
    /// <param name="generation"></param>
    /// <param name="fitnessFunc"></param>
    /// <returns></returns>
    public static Individual[][] MatchingLeaderChoice(Individual[] generation, Func<float[], float> fitnessFunc)
    {
        /* Avoid messing up the original array by cloning it. */
        var couples = new List<Individual[]>();
        var source = ( (Individual[]) generation.Clone())
                // Apply the fitness function once before starting to make sure the function is consistent
                .Select(r => { r.fitness = fitnessFunc(r.achievements); return r; })
                .OrderByDescending(r => r.fitness).ToList()
                ;
        while (source.Count() > 1) {
            var leader = source.First();
            var leaderCompetence = leader.achievements.ToList().IndexOf(leader.achievements.Max());
            var chosen = source
                    .Where(r => !r.Equals(leader))
                    // Disregard the feature the leader is better at
                    .Select(r =>
                    {
                        r.achievements[leaderCompetence] = r.achievements[leaderCompetence]/10;
                        r.fitness = fitnessFunc(r.achievements);
                        return r;
                    })
                    .OrderByDescending(r => r.fitness)
                    // Return the list to it's original state
                    .Select(r =>
                    {
                        r.achievements[leaderCompetence] = r.achievements[leaderCompetence]*10;
                        r.fitness = fitnessFunc(r.achievements);
                        return r;
                    })
                    .First()
                ;
            couples.Add(new[]{leader, chosen});
            source.Remove(leader);
            source.Remove(chosen);
        }
        return couples.ToArray();
    }

    public static float[] OnePointCrossOver(float[] genomeA, float[] genomeB)
    {
        return KPointCrossOver(genomeA, genomeB, 1);
    }

    public static float[] KPointCrossOver(float[] genomeA, float[] genomeB, int k)
    {
        var result = new float[genomeA.Length];
        var random = new Random(Time.UnixNow());
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
        var random = new Random(Time.UnixNow());
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
        var random = new Random(Time.UnixNow());
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

    /// <summary>
    /// Given an array of pairs of genomes, apply a crossover function to each twice, generating a number of offspring
    /// equal to double the Length of the matches array, that is, the same size of the original population.
    /// </summary>
    /// <param name="matches">An Array of Matches. Each match is an array of Genomes containing 2 genomes. Each Genome
    /// is an Array of floats.</param>
    /// <param name="crossoverFunction">A function that receives two genomes and return one.</param>
    /// <returns>An array of genomes.</returns>
    private static float[][] _breedMatches(float[][][] matches, Func<float[], float[], float[]> crossoverFunction)
    {
        var offspring = new float[matches.Length * 2][];
        for (int i = 0; i < matches.Length; i++)
        {
            var first = i * 2;
            var second = first + 1;
            offspring[first] = crossoverFunction(matches[i][0], matches[i][1]);
            offspring[second] = crossoverFunction(matches[i][0], matches[i][1]);
        }
        return offspring;
    }

}

}
