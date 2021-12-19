using System.Collections;
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
            var rootObject = new GameObject();
            _geneticAlgorithm = rootObject.AddComponent<GeneticAlgorithm.GeneticAlgorithm>();
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
