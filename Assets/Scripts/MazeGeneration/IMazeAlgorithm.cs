using Utils;

namespace MazeGeneration
{
    /// <summary>
    /// An interface for a maze algorithm so that the code can evolve independently from the choices of maze algorithms.
    /// </summary>
    public interface IMazeAlgorithm
    {
        public Position[,] CreateMaze();
    }
}