using UnityEngine;
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
}
}
