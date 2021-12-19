using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class TestGeneticAlgorithm
{
    private GeneticAlgorithm _geneticAlgorithm;

    [OneTimeSetUp]
    public void Setup()
    {
        var rootObject = new GameObject();
        _geneticAlgorithm = rootObject.AddComponent<GeneticAlgorithm>();
    }

    // A Test behaves as an ordinary method
    [Test]
    public void TestGeneticAlgorithmExists()
    {
        Assert.IsNotNull(_geneticAlgorithm);
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
