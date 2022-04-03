using System;
using NUnit.Framework;
using Utils;
using Assert = UnityEngine.Assertions.Assert;

public class Case
{
    public readonly float result;
    public float X;
    public float Y;

    public Case(float x, float y, float result)
    {
        X = x;
        Y = y;
        this.result = result;
    }
}

public class TestUtils
{
    [OneTimeSetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSymmetricalApply()
    {
        var cases = new[]
        {
            // even number of elements
            new[] {new Case(1f, 2f, 2), new Case(2f, 1f, 1)},
            // odd number of elements
            new[] {new Case(1f, 2f, 2), new Case(2f, 1f, 1), new Case(1f, 2f, 2)}
        };
        foreach (var c in cases)
        {
            Iterables.SymmetricalApply(c, r => r.X += 1, r => r.X -= 1);
            Assert.AreApproximatelyEqual(2, c[0].X, "add to the first");
            Assert.AreApproximatelyEqual(1, c[1].X, "subtract to the second");
            if (c.Length > 2) Assert.AreApproximatelyEqual(2, c[2].X, "add to the last odd");
        }
    }

    [Test]
    public void TestBalancedRange()
    {
        var range = Iterables.BalancedRange(3);
        Assert.AreEqual(-1, range[0]);
        Assert.AreEqual(0, range[1]);
        Assert.AreEqual(1, range[2]);
        var rand = new Random();
        for (var i = 0; i < 100; i++)
        {
            var randomRange = Iterables.BalancedRange(rand.Next(1, 60));
            var difference = randomRange[randomRange.Length - 1] + randomRange[0];
            Assert.IsTrue(difference == 0 || difference == -1);
        }
    }

    [Test]
    public void TestCreateGrid()
    {
        var grid = Iterables.CreateGrid(
            new[] {1, 2, 3},
            new[] {9, 8, 7}
        );
        Assert.IsTrue(grid.Length == 9);
        Assert.IsTrue(grid[0, 0][0] == 1);
        Assert.IsTrue(grid[0, 0][1] == 9);
        Assert.IsTrue(grid[1, 0][0] == 2);
        Assert.IsTrue(grid[1, 0][1] == 9);
        Assert.IsTrue(grid[0, 1][0] == 1);
        Assert.IsTrue(grid[0, 1][1] == 8);
        Assert.IsTrue(grid[1, 1][0] == 2);
        Assert.IsTrue(grid[1, 1][1] == 8);
    }
}