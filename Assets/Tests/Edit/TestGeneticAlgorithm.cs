using System;
using System.Linq;
using GeneticAlgorithm;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
    public class TestGeneticAlgorithm
    {
        // this variable creates a standard genome
        private static readonly float[] _oneThroughFiveGenome =
        {
            1, 2, 3, 4, 5, 1, 2, 3, 4, 5,
            1, 2, 3, 4, 5, 1, 2, 3, 4, 5,
            1, 2, 3, 4, 5, 1, 2, 3, 4, 5,
            1, 2, 3, 4, 5
        };

        private static readonly float[] _allOnes =
        {
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1
        };

        private static readonly float[] _allNines =
        {
            9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
            9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
            9, 9, 9, 9, 9
        };

        private GeneticAlgorithm.GeneticAlgorithm _geneticAlgorithm;

        [OneTimeSetUp]
        public void Setup()
        {
            _geneticAlgorithm = new GeneticAlgorithm.GeneticAlgorithm();
        }

        [Test]
        public void TestGeneticAlgorithmExists()
        {
            Assert.IsNotNull(_geneticAlgorithm);
        }

        [Test]
        public void TestGeneticAlgorithmCreatesRandomGenome()
        {
            var randomShipGenome = _geneticAlgorithm.GenerateRandomShip();
            Assert.IsNotNull(randomShipGenome);
            Assert.IsTrue(randomShipGenome.Length == 35);
            foreach (var i in randomShipGenome) Assert.IsTrue(i != 0, "A gene cannot be null");
        }

        [Test]
        public void TestGeneticAlgorithmParsesGenome()
        {
            var shipGenome = new ShipGenome(_oneThroughFiveGenome);
            Assert.IsNotNull(shipGenome.Body, "Should have created a body");
            Assert.IsNotNull(shipGenome.Bridge, "Should have created a bridge");
            Assert.IsNotNull(shipGenome.LaserCannon, "Should have created a laserCannon");
            Assert.IsNotNull(shipGenome.MissileLauncher, "Should have created a missileLauncher");
            Assert.IsNotNull(shipGenome.Tractor, "Should have created a tractor");
            Assert.IsNotNull(shipGenome.Turbine, "Should have created a turbine");
            Assert.IsNotNull(shipGenome.Wing, "Should have created a wing");
        }

        [Test]
        public void TestGeneticAlgorithmCrossOver()
        {
            var shipGenomeA = new ShipGenome(_allOnes);
            var shipGenomeB = new ShipGenome(_allNines);
            var uniformCrossOver = GeneticAlgorithm.GeneticAlgorithm.UniformCrossOver(
                shipGenomeA.GetGenome(),
                shipGenomeB.GetGenome());
            Assert.IsTrue(uniformCrossOver.Average() > 1, "Average must be higher than the lower genome.");
            Assert.IsTrue(uniformCrossOver.Average() < 9, "Average must be lower than the higher genome.");
            var onePointCrossOver = GeneticAlgorithm.GeneticAlgorithm.OnePointCrossOver(
                shipGenomeA.GetGenome(),
                shipGenomeB.GetGenome());
            Assert.IsTrue(onePointCrossOver.Average() > 1, "Average must be higher than the lower genome.");
            Assert.IsTrue(onePointCrossOver.Average() < 9, "Average must be lower than the higher genome.");
            Assert.AreApproximatelyEqual(1f, onePointCrossOver[0], "First should be from A");
            Assert.AreApproximatelyEqual(9f, onePointCrossOver[onePointCrossOver.Length - 1], "Last should be from B");
            var twoPointCrossOver = GeneticAlgorithm.GeneticAlgorithm.KPointCrossOver(
                shipGenomeA.GetGenome(),
                shipGenomeB.GetGenome(),
                2);
            Assert.IsTrue(twoPointCrossOver.Average() > 1, "Average must be higher than the lower genome.");
            Assert.IsTrue(twoPointCrossOver.Average() < 9, "Average must be lower than the higher genome.");
            Assert.AreApproximatelyEqual(1f, onePointCrossOver[0], "First must be from A");
            Assert.AreApproximatelyEqual(1f, onePointCrossOver[0], "Last must be from A");
        }

        [Test]
        public void TestMutation()
        {
            var shipGenome = new ShipGenome(_allOnes).GetGenome();
            var mutated = GeneticAlgorithm.GeneticAlgorithm.Mutation(
                shipGenome, 1, 10
            );
            for (var i = 0; i < shipGenome.Length; i++)
                Assert.AreNotApproximatelyEqual(
                    shipGenome[i],
                    mutated[i],
                    "No gene should be equal"
                );
        }

        [Test]
        public void TestEvaluateGeneration()
        {
            var generation = _createGeneration(10);
            var evaluated = GeneticAlgorithm.GeneticAlgorithm.EvaluateGeneration(
                generation, r => r.Sum()
            );
            Assert.AreEqual(evaluated[0].Fitness, 27);
            Assert.AreEqual(evaluated[1].Fitness, 24);
            Assert.AreEqual(evaluated[8].Fitness, 3);
            Assert.AreEqual(evaluated[9].Fitness, 0);
        }

        [Test]
        public void TestSelectionRoulette()
        {
            var generation = new Individual[10];
            for (var i = 0; i < generation.Length; i++)
                generation[i] = new Individual
                {
                    Genes = new float[] {i, i, i},
                    Achievements = new float[] {i, i, i},
                    Fitness = (float) Math.Pow(i, 5)
                };
            var selected = GeneticAlgorithm.GeneticAlgorithm.SelectionRoulette(generation);
            var previousGenAvg = generation.Average(r => r.Fitness);
            var nextGenAvg = selected.Average(r => r.Fitness);
            Assert.AreNotApproximatelyEqual(previousGenAvg, nextGenAvg);
            Assert.IsTrue(nextGenAvg > previousGenAvg);
        }

        [Test]
        public void TestMatchingLeaderChoice()
        {
            var generation = _createGeneration(6);
            generation[0].Achievements[0] = 9999f;
            generation[1].Achievements[0] = 8888f;
            generation[2].Achievements[1] = 7777f;
            generation[3].Achievements[1] = 6666f;
            generation[4].Achievements[2] = 5555f;
            generation[5].Achievements[2] = 4444f;
            var matches = GeneticAlgorithm.GeneticAlgorithm.MatchingLeaderChoice(
                generation, floats => floats.Sum()
            );
            Assert.AreEqual(matches[0][0], generation[0]);
            Assert.AreEqual(matches[0][1], generation[2]);
            Assert.AreEqual(matches[1][0], generation[1]);
            Assert.AreEqual(matches[1][1], generation[3]);
            Assert.AreEqual(matches[2][0], generation[4]);
            Assert.AreEqual(matches[2][1], generation[5]);
        }

        /// <summary>
        ///     Helper function to create a generation for tests.
        ///     Each created individual is identified by having it's index value as each of the gene and achievements value.
        /// </summary>
        /// <param name="numberOfIndividuals"></param>
        private static Individual[] _createGeneration(int numberOfIndividuals)
        {
            var result = new Individual[numberOfIndividuals];
            for (var i = 0; i < numberOfIndividuals; i++)
                result[i] = new Individual
                {
                    Genes = new float[] {i, i, i},
                    Achievements = new float[] {i, i, i}
                };
            return result;
        }
    }
}