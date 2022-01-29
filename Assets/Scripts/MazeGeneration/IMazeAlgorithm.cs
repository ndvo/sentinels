using Utils;

namespace MazeGeneration
{
    public interface IMazeAlgorithm
    {
        public Position[,] CreateMaze();
    }
}