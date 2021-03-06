using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace GeneticAlgorithm
{
    /// <summary>
    ///     The GeneticAlgorithm class manages the Execution of the genetic algorithm.
    ///     The idea is that a solution for a problem is encoded and evolved over generations starting from a random starting
    ///     point.
    ///     The problem, in our case, is finding an enemy ship, mine or missile that best achieves its goals.
    ///     The solution is encoded as an array of floating point numbers.
    ///     Each number is a gene and can be decoded into a feature.
    ///     Each array of genes is a genome and can be decoded into a full solution.
    ///     An individual is a solution with a record of achievements (a solution that has already performed and can be
    ///     evaluated).
    ///     A generation is a set of genomes.
    /// </summary>
    public class GeneticAlgorithm
    {
        
        /// <summary>
        ///     Create a random ship genome.
        /// </summary>
        /// <returns>a random genome</returns>
        public static float[] GenerateRandomShip()
        {
            var genome = new float[35];
            for (var i = 0; i < 35; i++) genome[i] = Random.value;
            return genome;
        }

        /// <summary>
        ///     Creates a Genetic Algorithm given the necessary functions.
        /// </summary>
        /// <param name="fitnessFunction">
        ///     a function that converts a list of achievements into a single float representing the
        ///     fitness value.
        /// </param>
        /// <param name="selectionFunction">a function that select a subset of the individuals.</param>
        /// <param name="matchingFunction">a function that creates matches of individuals to derive new ones.</param>
        /// <param name="crossoverFunction">a function that creates a new genome from two parent genomes.</param>
        /// <returns>a function that returns a new generation when provided with the previous generation.</returns>
        public static Func<Individual[], float[][]> GaFactory(
            Func<float[], float> fitnessFunction,
            Func<Individual[], Individual[]> selectionFunction,
            Func<Individual[], Func<float[], float>, Individual[][]> matchingFunction,
            Func<float[], float[], float[]> crossoverFunction
        )
        {
            return individuals => NewGeneration(
                individuals,
                fitnessFunction, selectionFunction, matchingFunction, crossoverFunction);
        }

        /// <summary>
        ///     Applies a fitness function to each individual ard return a sorted array of fittest.
        /// </summary>
        /// <param name="generation"></param>
        /// <param name="fitness"></param>
        /// <returns>The generation ordered by descending order of the fitness value.</returns>
        public static Individual[] EvaluateGeneration(Individual[] generation, Func<float[], float> fitness)
        {
            foreach (var ind in generation) ind.Fitness = fitness(ind.Achievements);
            return generation.OrderByDescending(r => r.Fitness).ToArray();
        }

        /// <summary>
        ///     Creates a new generation of individuals.
        ///     This function implements the Genetic Algorithm pipeline.
        ///     1- Applies a fitness function to the given generation (an array of Individuals)
        ///     2- Applies a selection function to the given generation (create a smaller array of Individuals: the survivors)
        ///     3- Applies a matching function to the survivors (create an array of matches)
        ///     4- Applies a crossover function to the matches (create an array of genomes)
        /// </summary>
        /// <param name="previousGeneration">
        ///     An array of individuals with both genes and achievements.
        /// </param>
        /// <param name="fitnessFunction">
        ///     The fitness function takes the individual's achievements and return a fitness value.
        /// </param>
        /// <param name="selectionFunction">
        ///     The selection function removes some individuals from the generation before the matching process.
        /// </param>
        /// <param name="matchingFunction">
        ///     The matching function needs to receive a generation and return couples.
        ///     The fitness function is passed into it so that it can be used to choose the pairs.
        /// </param>
        /// <param name="crossoverFunction"></param>
        /// <returns>the new generation</returns>
        private static float[][] NewGeneration(
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
            foreach (var i in previousGeneration) i.Fitness = fitnessFunction(i.Achievements);
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
                matches.Select(r => new[] {r[0].Genes, r[1].Genes}).ToArray(),
                crossoverFunction);
            /* 5- Mutate:  
            */
            return offspring.Select(r => Mutation(r)).ToArray();
        }

        /// <summary>
        ///     Selection algorithms receive as input a Generation (array of individuals) and output an array of surviving
        ///     Individuals.
        ///     Roulette selection accomplishes this by giving individuals a change of being selected as high as their
        ///     proportional fitness value.
        ///     Please, notice that the Roulette Selection makes it likely for the same individual to be over represented in
        ///     the next generation. Each draw from the pool takes into consideration the initial probabilities. Nothing else.
        /// </summary>
        /// <param name="generation">
        ///     The generation to be selected from. The generation must have been already evaluated.
        /// </param>
        /// <param name="deathRate">The proportion of the individuals that are not going to be selected.</param>
        /// <returns>An array of selected individuals.</returns>
        public static Individual[] SelectionRoulette(Individual[] generation, float deathRate = 0.2f)
        {
            var fitnessSum = generation.Sum(r => r.Fitness);
            fitnessSum = fitnessSum == 0 ? 1 : fitnessSum; // avoid division per zero
            var ranking = new float[generation.Length];
            var previous = 0f;
            for (var i = 0; i < generation.Length; i++)
            {
                ranking[i] = generation[i].Fitness / fitnessSum + previous;
                previous = ranking[i];
            }

            var selectionSize = (int) (generation.Length * (1 - deathRate));
            var selected = new Individual[selectionSize];
            for (var i = 0; i < selectionSize; i++)
            {
                var chosenValue = Random.value;
                for (var ii = 0; ii < generation.Length; ii++)
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
        ///     Create couples with complementary features following the leader's choice.
        ///     Take the first ranked Individual and chooses a couple with the highest
        /// </summary>
        /// <param name="generation"></param>
        /// <param name="fitnessFunc"></param>
        /// <returns></returns>
        public static Individual[][] MatchingLeaderChoice(Individual[] generation, Func<float[], float> fitnessFunc)
        {
            /* Avoid messing up the original array by cloning it. */
            var couples = new List<Individual[]>();
            var generationCopy = new Individual[generation.Length];
            for (var i = 0; i < generation.Length; i++)
            {
                var ind = generation[i];
                ind.Fitness = fitnessFunc(ind.Achievements);
                generationCopy[i] = ind;
            }

            var source = generationCopy.OrderByDescending(r => r.Fitness).ToList();
            while (source.Count > 1)
            {
                var leader = source.First();
                var leaderCompetence = leader.Achievements.ToList().IndexOf(leader.Achievements.Max());
                var notLeaders = source.Where(r => r.GetHashCode() != leader.GetHashCode()).ToArray();
                if (notLeaders.Length == 0)
                {
                    couples.Add(MakeCouple(leader, source.Last()));
                    continue;
                }

                var reEvaluated = notLeaders
                    // Disregard the feature the leader is better at
                    .Select(r =>
                    {
                        r.Achievements[leaderCompetence] = r.Achievements[leaderCompetence] / 10;
                        r.Fitness = fitnessFunc(r.Achievements);
                        return r;
                    }).ToArray();
                var corrected = reEvaluated
                    .OrderByDescending(r => r.Fitness)
                    // Return the list to it's original state
                    .Select(r =>
                    {
                        r.Achievements[leaderCompetence] = r.Achievements[leaderCompetence] * 10;
                        r.Fitness = fitnessFunc(r.Achievements);
                        return r;
                    }).ToArray();
                var chosen = corrected.First();
                couples.Add(MakeCouple(leader, chosen));
            }

            return couples.ToArray();

            // scoped function
            Individual[] MakeCouple(Individual a, Individual b)
            {
                source.Remove(a);
                source.Remove(b);
                return new[] {a, b};
            }
        }

        /// <summary>
        ///     Implements an algorithm to cross over two set of genes by picking all genes from the first one up to a certain
        ///     point and taking genes from the other one from there on.
        /// </summary>
        /// <param name="genomeA"></param>
        /// <param name="genomeB"></param>
        /// <returns>A new genome inheriting from genomeA and genomeB</returns>
        public static float[] OnePointCrossOver(float[] genomeA, float[] genomeB)
        {
            return KPointCrossOver(genomeA, genomeB, 1);
        }

        /// <summary>
        ///     Implements an algorithm to cross over two set of genes by setting K points
        /// </summary>
        /// <param name="genomeA"></param>
        /// <param name="genomeB"></param>
        /// <param name="k"></param>
        /// <returns>The new genome inheriting from genomeA and genomeB</returns>
        public static float[] KPointCrossOver(float[] genomeA, float[] genomeB, int k)
        {
            var result = new float[genomeA.Length];
            var kPoints = new int[k];
            for (var i = 0; i < k; i++) kPoints[i] = Random.Range(0, genomeA.Length);
            Array.Sort(kPoints);
            var coin = 0;
            for (var i = 0; i < genomeA.Length; i++)
            {
                // if this corresponds to a k point, flip the coin
                if (Array.IndexOf(kPoints, i) != -1) coin = (coin + 1) % 2;
                // get the gene from the genome indicated by the coin
                var gene = coin == 1 ? genomeB[i] : genomeA[i];
                result[i] = gene;
            }

            return result;
        }

        /// <summary>
        ///     Implements a crossover function that randomly pick each gene from a parent.
        /// </summary>
        /// <param name="genomeA"></param>
        /// <param name="genomeB"></param>
        /// <returns>The new genome inheriting from genomeA and genomeB</returns>
        public static float[] UniformCrossOver(float[] genomeA, float[] genomeB)
        {
            var result = new float[genomeA.Length];
            for (var i = 0; i < genomeA.Length; i++)
            {
                var coin = Random.Range(1, 3);
                var gene = coin == 1 ? genomeA[i] : genomeB[i];
                result[i] = gene;
            }

            return result;
        }

        /// <summary>
        ///     Mutates with given probability each of the genes from the genome.
        ///     Mutation occur randomly. For each gene a random number between 0 and 1 is chosen and if it is greater than
        ///     mutationProbability the gene is mutated.
        ///     Each mutation is capped to an increment of 0.02, but its value is random. The increment cap can be configured
        ///     by setting mutationIncrement.
        /// </summary>
        /// <param name="genome">The genome to be mutated.</param>
        /// <param name="mutationProbability">The probability of each mutation.</param>
        /// <param name="mutationIncrement">The maximum increment (or decrement) of each mutation.</param>
        /// <returns>the mutated genome</returns>
        public static float[] Mutation(
            float[] genome,
            float mutationProbability = 0.01f,
            float mutationIncrement = 0.02f
        )
        {
            var result = new float[genome.Length];
            for (var i = 0; i < genome.Length; i++)
            {
                var increment = 0f;
                if (Random.value < mutationProbability)
                {
                    var decrementModifier = Random.value >= 0.5 ? -1 : 1;
                    increment = Random.value * mutationIncrement * decrementModifier;
                }

                result[i] = genome[i] + increment;
            }

            return result;
        }

        /// <summary>
        ///     Given an array of pairs of genomes, apply a crossover function to each twice, generating a number of offspring
        ///     equal to double the Length of the matches array, that is, the same size of the original population.
        /// </summary>
        /// <param name="matches">
        ///     An Array of Matches. Each match is an array of Genomes containing 2 genomes. Each Genome
        ///     is an Array of floats.
        /// </param>
        /// <param name="crossoverFunction">A function that receives two genomes and return one.</param>
        /// <returns>An array of genomes.</returns>
        private static IEnumerable<float[]> _breedMatches(IReadOnlyList<float[][]> matches, Func<float[], float[], float[]> crossoverFunction)
        {
            var offspring = new float[matches.Count * 2][];
            for (var i = 0; i < matches.Count; i++)
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