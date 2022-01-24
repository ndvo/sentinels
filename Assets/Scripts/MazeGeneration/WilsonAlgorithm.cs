using Random = System.Random;

namespace MazeGeneration
{
    public readonly struct Position
    {
        public readonly int x;
        public readonly int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.x + b.x, a.y + b.y);
        }
    }

    /// <summary>
    /// Wilson's algorithm creates an unbiased maze.
    ///
    /// Mazes generated by this algorithm will have dead ends, but will not have a bias towards straightways or tuns.
    ///
    /// Implementation:
    /// Wilson's Algorithm is a maze generation algorithm that basically starts with all walls and starts creating a
    /// path from a random point. From the starting point, chooses
    ///
    /// References:
    /// <see>https://en.wikipedia.org/wiki/Maze_generation_algorithm</see>
    /// <see>https://people.cs.ksu.edu/~ashley78/wiki.ashleycoleman.me/index.php/Wilson's_Algorithm.html</see>
    /// </summary>
    public class WilsonAlgorithm
    {
        private readonly Position[,] _maze;
        private bool[,] _visitedBoard;
        private readonly Random _random = new Random(SentinelsUtils.Time.UnixNow());
        public readonly int sizeX;
        public readonly int sizeY;

        public WilsonAlgorithm(int sizeX, int sizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            _maze = new Position[sizeX, sizeY];
            ;
        }

        public Position[,] CreateMaze()
        {
            _visitedBoard = _createBoard();
            var currentCell = _randomPosition();
            _visitedBoard[currentCell.x, currentCell.y] = true;
            var mazeLength = sizeX * sizeY;
            var mazeCreated = 1;
            while (mazeCreated < mazeLength)
            {
                currentCell = _randomPosition();
                _createPath(currentCell);
                mazeCreated += _addToVisited(currentCell);
            }
            return _maze;
        }

        /// <summary>
        /// A Path is an array of Positions.
        ///
        /// The first position is the initial position of the Path.
        /// The next positions are the directions one should take in this path.
        ///
        /// This function has two parts that might have been better implemented in separate methods.
        /// The two parts are in this same function because we would need to return both the initial 
        /// </summary>
        /// <param name="startPos"></param>
        /// <returns></returns>
        private Position[,] _createPath(Position startPos)
        {
            var currentCell = startPos;
            do
            {
                var direction = _randomDirection();
                var adjacent = currentCell + direction;
                _maze[currentCell.x, currentCell.y] = direction;
                currentCell = adjacent;
            } while (!_isPositionInBoard(currentCell, _visitedBoard));

            return _maze;
        }

        /// <summary>
        /// Add to the visited list all the positions in a path starting at startPos.
        ///
        /// Starting from startPos, follow the directions adding the positions to the visited list.
        /// Returns the number of items added to allow avoiding counting the items in _maze.
        /// </summary>
        /// <param name="startPos">The position from witch to start adding items to the visited list.</param>
        /// <returns>The number of items added to visited.</returns>
        private int _addToVisited(Position startPos)
        {
            var count = 0;
            var currentCell = startPos;
            while (!_isPositionInBoard(currentCell, _visitedBoard))
            {
                _visitedBoard[currentCell.x, currentCell.y] = true;
                currentCell += _maze[currentCell.x, currentCell.y];
                count++;
            }
            return count;
        }

        /// <summary>
        /// Verifies that a given position is already present in a board.
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="board"></param>
        /// <returns>True if the position is already in the board.</returns>
        private bool _isPositionInBoard(Position p, bool[,] board)
        {
            return board[p.x, p.y];
        }

        private Position _randomPosition()
        {
            return new Position(
                _random.Next(0, sizeX),
                _random.Next(0, sizeY)
            );
        }

        /// <summary>
        /// Creates a random direction.
        /// </summary>
        /// <returns>A random direction</returns>
        private Position _randomDirection()
        {
            int incrementX;
            int incrementY;
            do
            {
                incrementX = _random.Next(-1, 1);
                incrementY = _random.Next(-1, 1);
            } while (
                // We need x and y to be different for it to be a direction.
                (incrementX == incrementY) 
                ||
                // We also need to exclude diagonals (-1,1), (1,-1)
                (incrementX + incrementY == 0)
                );
            return new Position(incrementX, incrementY);
        }


        /// <summary>
        /// Create a maze board of size X, Y
        ///
        /// The board is represented with a 2 dimensional int bool array.
        /// </summary>
        /// <returns>A clean board with all fields false</returns>
        private bool[,] _createBoard()
        {
            return new bool[sizeX, sizeY];
        }
        
        
    }
}
