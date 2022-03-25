using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class TestAttackSentinel
{

    private readonly GameObject _sentinel = AssetDatabase.LoadAssetAtPath<GameObject>(
        "Assets/Prefabs/SentinelShip/AstroEagle17.prefab");
    private readonly GameObject _protonLegacy = AssetDatabase.LoadAssetAtPath<GameObject>(
        "Assets/Prefabs/Ships/ProtonLegacy1.prefab");
    
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestFireOnProximity()
    {
        var enemy = Object.Instantiate(_protonLegacy, Vector3.zero, Quaternion.identity);
        enemy.AddComponent<AudioListener>();
        var attackSentinel = enemy.GetComponent<AttackSentinel>();
        attackSentinel.attackProbability = 100f;
        Object.Instantiate(_sentinel, new Vector3(2, 2, 2), Quaternion.identity);
        
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return new WaitForSeconds(1f);
        var projectile = GameObject.FindWithTag("Projectile");
        Assert.IsNotNull(projectile);
    }
}
