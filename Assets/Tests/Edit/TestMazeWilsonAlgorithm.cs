using MazeGeneration;
using NUnit.Framework;

public class TestMazeWilsonAlgorithm
{
    private MazeGeneration.WilsonAlgorithm _mazeWilson;
    
    [OneTimeSetUp]
    public void Setup()
    {
        _mazeWilson = new WilsonAlgorithm(3, 3);
    }

    [Test]
    public void TestMazeWilsonAlgorithmExists()
    {
        Assert.IsNotNull(_mazeWilson);
    }

    [Test]
    public void TestMazeWilsonCreateMazeWithGivenSizes()
    {
        var sizeX = 3;
        var sizeY = 3;
        var mazeGenerator = new MazeGeneration.WilsonAlgorithm(sizeX, sizeY);
        var maze = mazeGenerator.CreateMaze();
        Assert.IsTrue(maze.Length == sizeX * sizeY);
        Assert.IsTrue(maze.GetLength(0) == mazeGenerator.sizeX);
        Assert.IsTrue(maze.GetLength(1) == mazeGenerator.sizeY);
    }

    [Test]
    public void TestMazeWilsonContainsDirections()
    {
        for (int a = 0; a < 100; a++)
        {
            var maze = _mazeWilson.CreateMaze();
            foreach (var i in maze)
            {
                Assert.IsTrue(i.x != 0 || i.y != 0);
                Assert.IsTrue(i.x != i.y );
            }
        }
    }

}
