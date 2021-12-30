using System;
using Random = System.Random;

namespace GeneticAlgorithm
{
    
public class GeneticAlgorithm 
{

    public float[] GenerateRandomShip()
    {
        var random = new Random(Utils.Time.UnixNow());
        var genome = new float[35];
        for (var i=0; i<35; i++)
        {
            genome[i] = (float) random.NextDouble();
        }
        return genome;
    }

    public static float[] OnePointCrossOver(float[] genomeA, float[] genomeB)
    {
        return KPointCrossOver(genomeA, genomeB, 1);
    }

    public static float[] KPointCrossOver(float[] genomeA, float[] genomeB, int k)
    {
        var result = new float[genomeA.Length];
        var random = new Random(Utils.Time.UnixNow());
        var kPoints = new int[k];
        for (var i = 0; i < k; i++) kPoints[i] = random.Next(0, genomeA.Length);
        Array.Sort(kPoints);
        var coin = 0;
        for (var i = 0; i < genomeA.Length; i++)
        {
            if (Array.IndexOf(kPoints, i) != -1) coin = (coin + 1) % 2;
            var gene = coin == 1 ? genomeB[i] : genomeA[i];
            result[i] = gene;
        }
        return result;
    }
    
    public static float[] UniformCrossOver(float[] genomeA, float[] genomeB)
    {
        var result = new float[genomeA.Length];
        var random = new Random(Utils.Time.UnixNow());
        for (var i = 0; i < genomeA.Length; i++)
        {
            var coin = random.Next(1, 3);
            var gene = coin == 1 ? genomeA[i] : genomeB[i];
            result[i] = gene;
        }
        return result;
    }
}
}
