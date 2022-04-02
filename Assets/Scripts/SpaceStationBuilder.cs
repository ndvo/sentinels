using System;
using System.Linq;
using MazeGeneration;
using Utils;
using UnityEngine;
using Object = UnityEngine.Object;

public class SpaceStationBuilder : MonoBehaviour
{
    public GameObject commonPrefab;
    public GameObject core1Prefab;
    public GameObject core2Prefab;
    public GameObject panelsPrefab;
    public GameObject regularConnectionPrefab;
    public GameObject tubularConnectionPrefab;
    private GameObject[] _corePrefabs;
    private GameObject[] _longPrefabs;
    private GameObject[] _shortPrefabs;
    private IMazeAlgorithm _mazeGenerator;
    [SerializeField]
    private float cellSize = 0.1f;
    [SerializeField]
    private int maxSize = 5;
    [SerializeField]
    private int minSize = 3;
    private readonly System.Random _random = new System.Random(Utils.Time.UnixNow());
    private int size;
    private GameObject border;

    public void Awake()
    {
        if (PlaySession.isPractice)
        {
            minSize += 2;
            maxSize += 2;
            cellSize -= 1;
        }

        size = _random.Next(minSize, maxSize);
        _mazeGenerator = new WilsonAlgorithm(size, size);
    }

    public void Start()
    {
        _setupPrefabs();
        border = _createBorderGameObject(Direction.North);
        var maze = _mazeGenerator.CreateMaze();
        var stationParts = (from i in 
                new int[maze.GetLength(0) * maze.GetLength(1)]
            select _createConnection()).ToArray();
        var positionGrid = Utils.Iterables.CreateGrid(
            (from n in  Utils.Iterables.BalancedRange(size) select (float) n * cellSize).ToArray(), 
            (from n in Utils.Iterables.BalancedRange(size) select (float) n * cellSize).ToArray() 
            );
        for (int i = 0; i < stationParts.Length; i++)
        {
            var col = Math.Abs(i % maze.GetLength(1));
            var row = Math.Abs(i / maze.GetLength(0));
            _rotatePiece(stationParts[i], maze[row, col]);
            _transformPosition(stationParts[i], positionGrid[row, col]);
        }
    }

    GameObject _createBorderGameObject(Position direction, float padding = 7)
    {
        var go = new GameObject(direction.ToString());
        go.transform.position = transform.position + Vector3.zero;
        go.transform.RotateAround(
            new Vector3(0f, 0f, 0f), 
            new Vector3(direction.y, 0, direction.x), (cellSize/2) * size + padding);
        return go;
    }

    /// <summary>
    /// Changes the position of a part making it relative to the space station builder object.
    ///
    /// makes the part.transform.position equal to this.transform.position plus the provided x and z values.
    /// Even though the game is 3D the action actually happens in a curved 2D plane around Earth.
    /// The z axis of the 3D world is the y axis of our virtual plane.
    /// </summary>
    /// <param name="part"></param>
    /// <param name="position"></param>
    private void _transformPosition(GameObject part, float[] position)
    {
        part.transform.RotateAround(new Vector3(0f, 0f, 0f), Vector3.right, (float) position[0]);
        part.transform.RotateAround(new Vector3(0f, 0f, 0f), Vector3.forward, (float) position[1]);
    }

    private void _setupPrefabs()
    {
        _shortPrefabs = new[] {commonPrefab, panelsPrefab};
        _corePrefabs = new[] {core1Prefab, core2Prefab};
        _longPrefabs = new[] {regularConnectionPrefab, tubularConnectionPrefab};
    }

    private void _rotatePiece(GameObject piece, Position direction)
    {
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

    public bool IsOffBoard(Vector3 position)
    {
        return position.y < border.transform.position.y;
    }

    public int GetBoardSize()
    {
        return size;
    }

}
