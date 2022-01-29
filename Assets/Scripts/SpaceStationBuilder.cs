using System.Collections;
using System.Collections.Generic;
using SentinelsUtils;
using UnityEditor;
using UnityEngine;

public class SpaceStationBuilder : MonoBehaviour
{
    private readonly string _spaceStationAssetFolder = "Assets/Prefabs/";
    private readonly string _prefabExtension = ".prefab";
    private GameObject _core1Prefab;
    private GameObject _core2Prefab;
    private GameObject _panelsPrefab;
    private GameObject _commonPrefab;
    private GameObject _regularConnectionPrefab;
    private GameObject _tubularConnectionPrefab;
    private GameObject[] _corePrefabs;
    private GameObject[] _shortPrefabs;
    private GameObject[] _longPrefabs;
    private readonly System.Random _random = new System.Random(SentinelsUtils.Time.UnixNow());
    
    void Start()
    {
        _core1Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            _spaceStationAssetFolder + "Core1" + _prefabExtension
        );
        _core2Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            _spaceStationAssetFolder + "Core2" + _prefabExtension
        );
        _panelsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            _spaceStationAssetFolder + "Panels" + _prefabExtension
        );
        _commonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            _spaceStationAssetFolder + "Common" + _prefabExtension
        );
        _regularConnectionPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            _spaceStationAssetFolder + "RegularConnection" + _prefabExtension
        );
        _tubularConnectionPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            _spaceStationAssetFolder + "TubularConnection" + _prefabExtension
        );
        _shortPrefabs = new[] {_commonPrefab, _panelsPrefab};
        _corePrefabs = new[] {_core1Prefab, _core2Prefab};
        _longPrefabs = new[] {_regularConnectionPrefab, _tubularConnectionPrefab};
    }

    private void _rotatePiece(GameObject piece, Position direction)
    {
        piece.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        if (direction == Direction.South) piece.transform.Rotate(new Vector3(0f, 180f, 0f));
        if (direction == Direction.East) piece.transform.Rotate(new Vector3(0f, 90f, 0f));
        if (direction == Direction.West) piece.transform.Rotate(new Vector3(0f, -90f, 0f));
    }

    private GameObject _createCore()
    {
        return _createObject(_corePrefabs);
    }

    private GameObject _createConnection()
    {
        return _createObject(_longPrefabs);
    }

    private GameObject _createRoom()
    {
        return _createObject(_shortPrefabs);
    }

    private GameObject _createObject(GameObject[] availablePrefabs)
    {
        var prefab = availablePrefabs[_random.Next(availablePrefabs.Length)];
        return Object.Instantiate(prefab);
    }
    


}