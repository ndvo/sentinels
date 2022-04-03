namespace GeneticAlgorithm
{
    /// <summary>
    ///     ShipPartGenome
    ///     Describe a genome for a single part of a ship.
    ///     A ship part does not map to a ship behaviour, but to parts in it's model.
    /// </summary>
    public class ShipPartGenome
    {
        private readonly int _maxParts;
        private readonly float _maxPosition;
        private readonly float _maxRotation;
        private readonly float _maxSize;

        private float[] _data;
        public float count;
        public float position;
        public float rotation;
        public float size;
        public int type;

        public ShipPartGenome(
            float[] data,
            float maxPosition = 5,
            int maxParts = 6,
            float maxRotation = 90,
            float maxSize = 2
        )
        {
            _maxPosition = maxPosition;
            _maxParts = maxParts;
            _maxRotation = maxRotation;
            _maxSize = maxSize;
            SetGene(data);
        }

        /// <summary>
        ///     Converts an array of floating pointing numbers into a semantically meaningful set of values to implement
        ///     ship features.
        ///     This method is the core of the conversion between the genome and the phenotype.
        ///     For this prototype this method is rather simple. It makes no distinction about which type of part this is
        ///     and it does little more than simply scaling the values into something that is useful for the ship.
        ///     This level of complexity is enough for this prototype as it allows the ship to visibly change in the tests.
        /// </summary>
        private void _setFeatures()
        {
            count = 1 + (int) _data[0] * _maxParts;
            position = _data[1] * _maxPosition;
            rotation = _data[2] * _maxRotation;
            size = _data[3] * _maxSize;
            type = (int) _data[4] * 3;
        }

        /// <summary>
        ///     Retrieves the data that generated the Ship Part.
        ///     This method can be done by either reversing the operations from _setFeatures or by storing the original
        ///     data.
        ///     We chose to store the original data and make _setFeatures explicitly use the stored data in every,
        ///     invocation, allowing mutations to be applied by changing the data and re-invoking the _setFeatures.
        /// </summary>
        /// <returns></returns>
        public float[] GetGene()
        {
            return _data;
        }

        /// <summary>
        ///     Changes the genes of the ShipPart and parses it to create the Features of the Phenotype.
        /// </summary>
        /// <param name="data"></param>
        public void SetGene(float[] data)
        {
            _data = data;
            _setFeatures();
        }
    }
}