using System.Collections;
using System.Linq;
using GeneticAlgorithm;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;


namespace Tests
{
    public class TestGeneticAlgorithm
    {
        // this variable creates a standard genome
        private static float[] _oneThroughFiveGenome = new float[] {
                1, 2, 3, 4, 5,   1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,   1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,   1, 2, 3, 4, 5,
                1, 2, 3, 4, 5
        };
        private GeneticAlgorithm.GeneticAlgorithm _geneticAlgorithm;

        [OneTimeSetUp]
        public void Setup()
        {
            _geneticAlgorithm = new GeneticAlgorithm.GeneticAlgorithm();
        }

        // A Test behaves as an ordinary method
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
            foreach (var i in randomShipGenome)
            {
                Assert.IsTrue(i != 0, "A gene cannot be null");
            }
        }

        [Test]
        public void TestGeneticAlgorithmParsesGenome()
        {
            var shipGenome = new GeneticAlgorithm.ShipGenome(TestGeneticAlgorithm._oneThroughFiveGenome);
            Assert.IsNotNull(shipGenome.body, "Should have created a body");
            Assert.IsNotNull(shipGenome.bridge, "Should have created a bridge");
            Assert.IsNotNull(shipGenome.laserCannon, "Should have created a laserCannon");
            Assert.IsNotNull(shipGenome.missileLauncher, "Should have created a missileLauncher");
            Assert.IsNotNull(shipGenome.tractor, "Should have created a tractor");
            Assert.IsNotNull(shipGenome.turbine, "Should have created a turbine");
            Assert.IsNotNull(shipGenome.wing, "Should have created a wing");
        }

        [Test]
        public void TestGeneticAlgorithmCrossOver()
        {
            var shipGenomeA = new GeneticAlgorithm.ShipGenome(new float[]
            {
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1,
            });
            var shipGenomeB = new GeneticAlgorithm.ShipGenome(new float[]
            {
                9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
                9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
                9, 9, 9, 9, 9,
            });
            var crossedOver = GeneticAlgorithm.GeneticAlgorithm.CrossOver(
                shipGenomeA.GetGenome(),
                shipGenomeB.GetGenome());
            Assert.IsTrue(crossedOver.Average() > 1, "Average must be higher than the lower genome.");
            Assert.IsTrue(crossedOver.Average() < 9, "Average must be lower than the higher genome.");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestGeneticAlgorithmWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
