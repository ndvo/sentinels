using System;
using Utils;

namespace MazeGeneration
{
    /// <summary>
    ///     Wilson's algorithm creates an unbiased maze.
    ///     Mazes generated by this algorithm will have dead ends, but will not have a bias towards straightways or turns.
    ///     Wilson's Algorithm is a maze generation algorithm that basically starts with all walls and starts creating a
    ///     path without loops from a random point up to another chosen random point. If the path hits a cell that was
    ///     already in the maze, it stops and open the hit wall.
    ///     References:
    ///     <see>https://en.wikipedia.org/wiki/Maze_generation_algorithm</see>
    ///     <see>https://people.cs.ksu.edu/~ashley78/wiki.ashleycoleman.me/index.php/Wilson's_Algorithm.html</see>
    /// </summary>
    public class WilsonAlgorithm : IMazeAlgorithm
    {
        private readonly Position[,] _maze;
        private readonly Random _random = new Random(Time.UnixNow());
        public readonly int SizeX;
        public readonly int SizeY;
        private bool[,] _visitedBoard;

        public WilsonAlgorithm(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            _maze = new Position[sizeX, sizeY];
        }

        public Position[,] CreateMaze()
        {
            _visitedBoard = _createBoard();
            var currentCell = _randomPosition();
            _visitedBoard[currentCell.X, currentCell.Y] = true;
            var mazeLength = SizeX * SizeY;
            var mazeCreated = 1;
            while (mazeCreated < mazeLength)
            {
                currentCell = _randomPosition();
                _updateMaze(currentCell);
                mazeCreated += _addToVisited(currentCell);
            }

            return _maze;
        }

        /// <summary>
        ///     A Path is an array of Positions.
        ///     The first position is the initial position of the Path.
        ///     The next positions are the directions one should take in this path.
        ///     This function has two parts that might have been better implemented in separate methods.
        ///     The two parts are in this same function because we would need to return both the initial
        /// </summary>
        /// <param name="startPos"></param>
        /// <returns></returns>
        private void _updateMaze(Position startPos)
        {
            var currentCell = startPos;
            while (true)
            {
                var direction = _randomDirection();
                _maze[currentCell.X, currentCell.Y] = direction;
                var adjacent = currentCell + direction;
                if (
                    !_validatePosition(adjacent)
                    ||
                    !_isPositionInBoard(adjacent, _visitedBoard)
                )
                    break;

                currentCell = adjacent;
            }
        }

        /// <summary>
        ///     Add to the visited list all the positions in a path starting at startPos.
        ///     Starting from startPos, follow the directions adding the positions to the visited list.
        ///     Returns the number of items added to allow avoiding counting the items in _maze.
        /// </summary>
        /// <param name="startPos">The position from witch to start adding items to the visited list.</param>
        /// <returns>The number of items added to visited.</returns>
        private int _addToVisited(Position startPos)
        {
            var count = 0;
            var currentCell = startPos;
            while (
                _validatePosition(currentCell)
                &&
                !_isPositionInBoard(currentCell, _visitedBoard)
            )
            {
                _visitedBoard[currentCell.X, currentCell.Y] = true;
                var nextCell = currentCell + _maze[currentCell.X, currentCell.Y];
                currentCell = nextCell;
                count++;
            }

            return count;
        }

        /// <summary>
        ///     Verifies that a given position is already present in a board.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="board"></param>
        /// <returns>True if the position is already in the board.</returns>
        private static bool _isPositionInBoard(Position p, bool[,] board)
        {
            return board[p.X, p.Y];
        }

        private Position _randomPosition()
        {
            return new Position(
                _random.Next(0, SizeX),
                _random.Next(0, SizeY)
            );
        }

        /// <summary>
        ///     Creates a random direction.
        /// </summary>
        /// <returns>A random direction</returns>
        private Position _randomDirection()
        {
            int incrementX;
            int incrementY;
            do
            {
                incrementX = _random.Next(-1, 2);
                incrementY = _random.Next(-1, 2);
            } while (
                // We also need to exclude diagonals (-1,1), (1,-1) and no movement (0, 0)
                Math.Abs(incrementX + incrementY) != 1
            );

            return new Position(incrementX, incrementY);
        }


        /// <summary>
        ///     Create a maze board of size X, Y
        ///     The board is represented with a 2 dimensional int bool array.
        /// </summary>
        /// <returns>A clean board with all fields false</returns>
        private bool[,] _createBoard()
        {
            return new bool[SizeX, SizeY];
        }

        private bool _validatePosition(Position pos)
        {
            return pos.X >= 0 && pos.X < SizeX &&
                   pos.Y >= 0 && pos.Y < SizeY;
        }
    }
}