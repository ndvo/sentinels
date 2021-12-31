using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

public class TestProtonLegacy
{
    private readonly GameObject _headquartersPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
        "Assets/Prefabs/EnemyHeadQuarters.prefab");
    private readonly GameObject _prefab = AssetDatabase.LoadAssetAtPath<GameObject>(
        "Assets/Prefabs/ProtonLegacy1.prefab");
    // A Test behaves as an ordinary method
    [Test]
    public void TestProtonLegacySimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestProtonLegacyWithEnumeratorApplyGenome()
    {
        var headquarters = Object.Instantiate(headquartersPrefab, new Vector3(), Quaternion.identity);
        headquarters.name = "EnemyHeadQuarters";
        var ship = Object.Instantiate(prefab, new Vector3(), Quaternion.identity);
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        var protonManager = ship.GetComponent<ProtonLegacy>();
        var body = protonManager.GetParts("ShipBody")[0];
        var bodyPosition = body.position.z;
        yield return null;
        var bodyPosition2 = body.position.z;
        Assert.AreNotEqual(bodyPosition, bodyPosition2, "Should have changed initial position.");


    }
}
