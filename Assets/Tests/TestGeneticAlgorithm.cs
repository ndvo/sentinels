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
            var shipGenome = new GeneticAlgorithm.ShipGenome(new float[]
            {
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
            });
            Assert.IsTrue(shipGenome.body.count == 1.0);
            Assert.IsTrue(shipGenome.bridge.position == 2.0);
            Assert.IsTrue(shipGenome.laserCannon.rotation == 3.0);
            Assert.IsTrue(shipGenome.missileLauncher.size == 4.0);
            Assert.IsTrue(shipGenome.tractor.type == 5.0);
            Assert.IsTrue(shipGenome.turbine.count == 1.0);
            Assert.IsTrue(shipGenome.wing.position == 2.0);
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
