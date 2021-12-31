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

    [UnityTest]
    public IEnumerator TestProtonLegacyWithEnumeratorApplyGenome()
    {
        var headquarters = Object.Instantiate(_headquartersPrefab, new Vector3(), Quaternion.identity);
        headquarters.name = "EnemyHeadQuarters";
        var ship = Object.Instantiate(_prefab, new Vector3(), Quaternion.identity);
        var protonManager = ship.GetComponent<ProtonLegacy>();
        var body = protonManager.GetParts("ShipBody")[0];
        var bodyPosition = body.position.z;
        yield return null;
        var bodyPosition2 = body.position.z;
        Assert.AreNotEqual(bodyPosition, bodyPosition2, "Should have changed initial position.");
    }
}
