using System;
using System.Linq;
using MazeGeneration;
using Utils;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// SpaceStation Builder is responsible for creating the game board.
/// </summary>
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
    // a border game object that allows one to check if a position is inside or outside the game board.
    private GameObject border;

    public void Awake()
    {
        if (PlaySession.isPractice)
        {
            // make the board larger
            minSize += 2;
            maxSize += 2;
            // make the space between stations shorter
            cellSize -= 1;
        }

        // determine the size of the game board
        size = _random.Next(minSize, maxSize);
        // determine the maze generation algorithm to be used
        _mazeGenerator = new WilsonAlgorithm(size, size);
    }

    public void Start()
    {
        _setupPrefabs();
        border = _createBorderGameObject(new Position(1, 1));
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

    /// <summary>
    /// Creates a game object to set the border of the board.
    /// </summary>
    /// <param name="direction">The direction for the boarder</param>
    /// <param name="padding">the space between the last tile and the border</param>
    /// <returns>the border game object</returns>
    GameObject _createBorderGameObject(Position direction, float padding = 2)
    {
        var go = new GameObject(direction.ToString());
        go.transform.position = transform.position + Vector3.zero;
        go.transform.RotateAround(
            new Vector3(0f, 0f, 0f), 
            new Vector3(direction.y, 0, direction.x),
            Mathf.Sqrt(2 * Mathf.Pow(cellSize * size/2, 2))  + padding);
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

    /// <summary>
    /// Rotate a single piece of the puzzle.
    /// </summary>
    /// <param name="piece">A piece of the puzzle, i.e. a game object representing a space station.</param>
    /// <param name="direction">The direction it should be rotated to.</param>
    private void _rotatePiece(GameObject piece, Position direction)
    {
        if (direction == Direction.South) piece.transform.Rotate(new Vector3(0f, 180f, 0f));
        if (direction == Direction.East) piece.transform.Rotate(new Vector3(0f, 90f, 0f));
        if (direction == Direction.West) piece.transform.Rotate(new Vector3(0f, -90f, 0f));
    }

    /// <summary>
    /// Create a connection piece.
    ///
    /// A connection piece is a "long" prefab. This is a game object that represents a "line" in a maze.
    /// A long object is capable of blocking the passage if connected to another long object.
    /// </summary>
    /// <returns>the connection object.</returns>
    private GameObject _createConnection()
    {
        return _createObject(_longPrefabs);
    }

    /// <summary>
    /// Create a random station.
    /// </summary>
    /// <param name="availablePrefabs"></param>
    /// <returns>A random piece of the puzzle.</returns>
    private GameObject _createObject(GameObject[] availablePrefabs)
    {
        var prefab = availablePrefabs[_random.Next(0, availablePrefabs.Length)];
        return Object.Instantiate(prefab, this.transform);
    }

    /// <summary>
    /// Checks if an object is inside the game board.
    /// </summary>
    /// <param name="position">The position to checked.</param>
    /// <returns>a boolean indicating if the position is within the board</returns>
    public bool IsOffBoard(Vector3 position)
    {
        // It is not necessary to check in all positions. Given the board is a part of a sphere, any position that is
        // lower in y than the border point is outside of the board game.
        return position.y < border.transform.position.y;
    }

}
