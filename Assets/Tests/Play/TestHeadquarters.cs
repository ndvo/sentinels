using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class TestHeadquarters
{
    private readonly GameObject _headquartersPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
        "Assets/Prefabs/EnemyHeadQuarters.prefab");

    private Headquarters _headquarterManager;
    private GameObject _headquartersGameObject;


    [OneTimeSetUp]
    public void Setup()
    {
        _headquartersGameObject = Object.Instantiate(
            _headquartersPrefab,
            new Vector3(),
            Quaternion.identity
        );
        _headquarterManager = _headquartersGameObject.GetComponent<Headquarters>();
    }

    [UnityTest]
    public IEnumerator TestHeadquartersCreateNewGenomes()
    {
        yield return null;
        var randomGenome = _headquarterManager.NewShipGenome();
        Assert.IsNotNull(randomGenome);
        Assert.IsNotNull(randomGenome.Body.count);
        Assert.IsNotNull(randomGenome.Body.rotation);
        Assert.IsNotNull(randomGenome.Body.type);
        Assert.IsNotNull(randomGenome.Bridge.position);
        Assert.IsNotNull(randomGenome.Tractor.size);
        Assert.IsTrue(_testGenome(randomGenome.GetGenome()));
    }

    [UnityTest]
    public IEnumerator TestHeadquartersCreateNewGeneration()
    {
        yield return null;
        var randomGeneration = _headquarterManager.NewShipGenomeGeneration(5);
        Assert.IsTrue(randomGeneration.Length == 5, "Should create the specified number of genomes.");
        Assert.IsTrue(randomGeneration.All(r => !(r is null)), "No generated ship is null");
        Assert.IsTrue(randomGeneration.All(r => _testGenome(r.GetGenome())), "Genomes should be valid.");
        Assert.IsTrue(randomGeneration.All(r => r.Body.GetGene().Length == 5), "Each feature is 5 genes long");
    }

    private static bool _testGenome(float[] genome)
    {
        Assert.IsTrue(genome.Length == 35, "Genomes should be 35 gene length.");
        Assert.IsTrue(genome.All(r => r >= 0f && r <= 1f), "Genes should be between 0 and 1.");
        return true;
    }
}