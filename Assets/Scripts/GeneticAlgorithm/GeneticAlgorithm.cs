using UnityEngine;
using Utils;
using Random = System.Random;

namespace GeneticAlgorithm
{
    
public class GeneticAlgorithm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float[] GenerateRandomShip()
    {
        var random = new Random(Utils.Time.UnixNow());
        var genome = new float[35];
        for (var i=0; i<35; i++)
        {
            genome[i] = random.Next();
        }
        return genome;
    }

    public static float[] CrossOver(float[] genomeA, float[] genomeB)
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
