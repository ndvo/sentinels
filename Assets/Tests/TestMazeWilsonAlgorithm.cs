using NUnit.Framework;

public class TestMazeWilsonAlgorithm
{
    private MazeGeneration.WilsonAlgorithm _mazeWilson;
    
    [OneTimeSetUp]
    public void Setup()
    {
        _mazeWilson = new MazeGeneration.WilsonAlgorithm(3, 3);
    }

    [Test]
    public void TestMazeWilsonAlgorithmExists()
    {
        Assert.IsNotNull(_mazeWilson);
    }

    [Test]
    public void TestMazeWilsonCreateMazeWithGivenSizes()
    {
        var maze = _mazeWilson.CreateMaze();
        Assert.Equals(maze.Length, 2);
        Assert.Equals(maze.GetLength(0), _mazeWilson.sizeX);
        Assert.Equals(maze.GetLength(1), _mazeWilson.sizeY);
    }

    [Test]
    public void TestMazeWilsonContainsDirections()
    {
        var maze = _mazeWilson.CreateMaze();
        foreach (var i in maze)
        {
            Assert.IsTrue(i.x != 0 || i.y != 0);
            Assert.IsTrue(i.x != i.y );
        }
    }

}
