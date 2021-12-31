using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

public class Case
{
    public float X;
    public float Y;
    public readonly float result;
    
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
        
        // A Test behaves as an ordinary method
        [Test]
        public void TestSymmetricalApply()
        {
            var cases = new[]
            {
                // even number of elements
                new [] {new Case(1f, 2f, 2), new Case(2f, 1f, 1)},
                // odd number of elements
                new [] {new Case(1f, 2f, 2), new Case(2f, 1f, 1), new Case(1f, 2f, 2)}, 
            };
            foreach (var c in cases)
            {
                Utils.Iterables.SymmetricalApply( c, (r) => r.X += 1, (r) => r.X -= 1 );
                Assert.AreApproximatelyEqual(2, c[0].X, "add to the first");
                Assert.AreApproximatelyEqual(1, c[1].X, "subtract to the second");
                if (c.Length > 2)
                {
                    Assert.AreApproximatelyEqual(2, c[2].X, "add to the last odd");
                }
            }
        }

}
