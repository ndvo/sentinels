using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
    
}
