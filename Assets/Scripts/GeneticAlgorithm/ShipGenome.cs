using System.Linq;
using Utils;

namespace GeneticAlgorithm
{
    public class ShipGenome
    {
        public ShipPartGenome body;
        public ShipPartGenome bridge;
        public ShipPartGenome laserCannon;
        public ShipPartGenome missileLauncher;
        public ShipPartGenome tractor;
        public ShipPartGenome turbine;
        public ShipPartGenome wing;
        
        public ShipGenome (float[] data)
        {
            body = new ShipPartGenome(data.SubArray(0, 5));
            bridge = new ShipPartGenome(data.SubArray(5, 5));
            laserCannon = new ShipPartGenome(data.SubArray(10, 5));
            missileLauncher = new ShipPartGenome(data.SubArray(15, 5));
            tractor = new ShipPartGenome(data.SubArray(20, 5));
            turbine = new ShipPartGenome(data.SubArray(25, 5));
            wing = new ShipPartGenome(data.SubArray(30, 5));
        }

        /// <summary>
        /// Convert from Genome to Array.
        /// </summary>
        /// <returns></returns>
        public float[] GetGenome()
        {
            return body.GetGene()
                .Concat(bridge.GetGene())
                .Concat(laserCannon.GetGene())
                .Concat(missileLauncher.GetGene())
                .Concat(tractor.GetGene())
                .Concat(turbine.GetGene())
                .Concat(wing.GetGene()).ToArray();
        }
    }

    public class ShipPartGenome
    {
        private float[] _data;
        public float count;
        public float position;
        public float rotation;
        public float size;
        public int type;

        private readonly int _maxParts;
        private readonly float _maxPosition;
        private readonly float _maxRotation;
        private readonly float _maxSize;

        public ShipPartGenome(
            float[] data, float maxPosition=4, int maxParts=6,
            float maxRotate=45, float maxSize=100
            )
        {
            _maxPosition = maxPosition;
            _maxParts = maxParts;
            _maxRotation = maxRotate;
            _maxSize = maxSize;
            SetGene(data);
        }

        private void _setFeatures()
        {
            count = 1 + (int) _data[0] * _maxParts;
            position = _data[1] * _maxPosition;
            rotation = _data[2] * _maxRotation;
            size = _data[3] * _maxSize;
            type = (int) _data[4] * 3;
        }

        public float[] GetGene()
        {
            return _data;
        }

        public void SetGene(float[] data)
        {
            _data = data;
            _setFeatures();
        }
    }

}