using MazeGeneration;
using NUnit.Framework;

public class TestMazeWilsonAlgorithm
{
    private WilsonAlgorithm _mazeWilson;

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
        var mazeGenerator = new WilsonAlgorithm(sizeX, sizeY);
        var maze = mazeGenerator.CreateMaze();
        Assert.IsTrue(maze.Length == sizeX * sizeY);
        Assert.IsTrue(maze.GetLength(0) == mazeGenerator.SizeX);
        Assert.IsTrue(maze.GetLength(1) == mazeGenerator.SizeY);
    }

    [Test]
    public void TestMazeWilsonContainsDirections()
    {
        for (var a = 0; a < 100; a++)
        {
            var maze = _mazeWilson.CreateMaze();
            foreach (var i in maze)
            {
                Assert.IsTrue(i.X != 0 || i.Y != 0);
                Assert.IsTrue(i.X != i.Y);
            }
        }
    }
}