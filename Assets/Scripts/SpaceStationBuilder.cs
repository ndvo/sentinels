using System;
using System.Linq;
using MazeGeneration;
using Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class SpaceStationBuilder : MonoBehaviour
{
    private GameObject _commonPrefab;
    private GameObject _core1Prefab;
    private GameObject _core2Prefab;
    private GameObject _panelsPrefab;
    private GameObject _regularConnectionPrefab;
    private GameObject _tubularConnectionPrefab;
    private GameObject[] _corePrefabs;
    private GameObject[] _longPrefabs;
    private GameObject[] _shortPrefabs;
    private IMazeAlgorithm _mazeGeneraton;
    private int _cellSize = 15;
    private int _maxSize = 5;
    private int _minSize = 3;
    private readonly System.Random _random = new System.Random(Utils.Time.UnixNow());
    private const string PrefabExtension = ".prefab";
    private const string SpaceStationAssetFolder = "Assets/Prefabs/SpaceStation/";

    public void SetStationsMaxSize(int newMaxSize, int newMinSize)
    {
        _maxSize = newMaxSize;
        _minSize = newMinSize;
    }

    public void Start()
    {
        _setupPrefabs();
        var sizeX = _random.Next(_minSize, _maxSize);
        var sizeY = _random.Next(_minSize, _maxSize);
        _mazeGeneraton = new WilsonAlgorithm(sizeX, sizeY);
        var maze = _mazeGeneraton.CreateMaze();
        var stationParts = (from i in 
                new int[maze.GetLength(0) * maze.GetLength(1)]
            select _createConnection()).ToArray();
        var positionGrid = Utils.Iterables.CreateGrid(
            (from n in  Utils.Iterables.BalancedRange(sizeX) select n * _cellSize).ToArray(), 
            (from n in Utils.Iterables.BalancedRange(sizeY) select n * _cellSize).ToArray() 
            );
        for (int i = 0; i < stationParts.Length; i++)
        {
            var col = Math.Abs(i % maze.GetLength(1));
            var row = Math.Abs(i / maze.GetLength(0));
            _transformPosition(stationParts[i], positionGrid[row, col]);
            _rotatePiece(stationParts[i], maze[row, col]);
        }
    }

    private void _transformPosition(GameObject part, int[] position)
    {
        part.transform.position += new Vector3(position[0], 0, position[1]);
    }

    private void _setupPrefabs()
    {
        _shortPrefabs = new[] {commonPrefab, panelsPrefab};
        _corePrefabs = new[] {core1Prefab, core2Prefab};
        _longPrefabs = new[] {regularConnectionPrefab, tubularConnectionPrefab};
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
        var prefab = availablePrefabs[_random.Next(0, availablePrefabs.Length)];
        return Object.Instantiate(prefab, this.transform);
    }

}
