using System.Linq;
using NUnit.Framework;
using Ships;
using UnityEngine;

namespace Tests
{
    public class TestMonteCarloTreeSearch
    {

        [Test]
        public void TestMonteCarloTreeSearchLongBranchShortPath()
        {
            var correctBranch = new[]
            {
                new TreeNode(true, false, new Vector3(0, 2, 0), new TreeNode[]
                {
                    // perfect match with blocked parent
                    new TreeNode(false, true, new Vector3(0, 0, 0), null)
                }),
                new TreeNode(false, false, new Vector3(1, 1, 2), new[]
                {
                    // best possible match
                    new TreeNode(false, true, new Vector3(0, 0, 1))
                }),
            };
            var incorrectBranch = new[]
            {
                new TreeNode(true, false, new Vector3(0, 2, 0), new TreeNode[]
                {
                    // perfect match with blocked parent and a closer parent
                    new TreeNode(false, true, new Vector3(0, 0, 0), null)
                }),
                new TreeNode(false, false, new Vector3(1, 1, 2), new[]
                {
                    // shorter total path with the leaf further thar best match
                    new TreeNode(false, true, new Vector3(0, 0, 2))
                }),
            };
            var tree = new TreeNode(false, false, new Vector3(5, 5, 5), new []
            {
                new TreeNode(false, false, new Vector3(5, 3, 2), correctBranch),
                new TreeNode(false, false, new Vector3(1, 2, 1), incorrectBranch),
            });
            var monteCarloSteer = new MonteCarloTreeSearch();
            var path = monteCarloSteer.ComputePath(tree, Vector3.zero);
            // Find path to the left
            Assert.AreEqual(tree.Children[0], path.Nodes[1]);
            tree = new TreeNode(false, false, new Vector3(5, 5, 5), new []
            {
                new TreeNode(false, false, new Vector3(1, 2, 1), incorrectBranch),
                new TreeNode(false, false, new Vector3(5, 3, 2), correctBranch),
            });
            path = monteCarloSteer.ComputePath(tree, Vector3.zero);
            // Find path to the right
            Assert.AreEqual(tree.Children[1], path.Nodes[1]);
        }
    
        [Test]
        public void TestMonteCarloTreeSearchReturnUnblocked()
        {
            var tree = new TreeNode(false, false, new Vector3(5, 5, 5), new []
            {
                new TreeNode(true, true, new Vector3(5, 3, 2), new TreeNode[] {}),
                new TreeNode(true, true, new Vector3(6, 3, 2), new TreeNode[] {}),
                new TreeNode(true, true, new Vector3(7, 3, 2), new TreeNode[] {}),
                new TreeNode(true, true, new Vector3(8, 3, 2), new TreeNode[] {}),
                new TreeNode(true, true, new Vector3(9, 3, 2), new TreeNode[] {}),
                new TreeNode(false, true, new Vector3(10, 3, 2), new TreeNode[] {}),
            });
            var monteCarloSteer = new MonteCarloTreeSearch();
            var path = monteCarloSteer.ComputePath(tree, Vector3.zero);
            Assert.AreEqual(tree.Children[5], path.Nodes[1]);
        }

        [Test]
        public void TestMonteCarloTreeSearchReturnShortest()
        {
            var tree = new TreeNode(false, false, new Vector3(5, 5, 5), new []
            {
                new TreeNode(false, true, new Vector3(5, 3, 2), new TreeNode[] {}),
                new TreeNode(false, true, new Vector3(6, 3, 2), new TreeNode[] {}),
                new TreeNode(false, true, new Vector3(7, 3, 2), new TreeNode[] {}),
                new TreeNode(false, true, new Vector3(8, 3, 2), new TreeNode[] {}),
                new TreeNode(false, true, new Vector3(9, 3, 2), new TreeNode[] {}),
                new TreeNode(false, true, new Vector3(10, 3, 2), new TreeNode[] {}),
            });
            var monteCarloSteer = new MonteCarloTreeSearch();
            var path = monteCarloSteer.ComputePath(tree, Vector3.zero);
            Assert.AreEqual(tree.Children[0], path.Nodes[1]);
        }
        
        [Test]
        public void TestMonteCarloTreeSearchReturnReached()
        {
            var tree = new TreeNode(false, false, new Vector3(5, 5, 5), new []
            {
                new TreeNode(false, false, new Vector3(5, 3, 2), new TreeNode[]
                {
                    new TreeNode(false, false, new Vector3(6, 3, 2), new TreeNode[]
                    {
                        new TreeNode(false, true, new Vector3(8, 3, 2), new TreeNode[] {}),
                        new TreeNode(false, true, new Vector3(9, 3, 2), new TreeNode[] {}),
                        new TreeNode(false, true, new Vector3(0, 0, 0), new TreeNode[] {}),
                    }),
                    new TreeNode(false, true, new Vector3(7, 3, 2), new TreeNode[] {}),
                }),
                new TreeNode(false, true, new Vector3(6, 3, 2), new TreeNode[] {}),
                new TreeNode(false, true, new Vector3(7, 3, 2), new TreeNode[] {}),
            });
            var monteCarloSteer = new MonteCarloTreeSearch();
            var path = monteCarloSteer.ComputePath(tree, Vector3.zero);
            Assert.AreEqual(tree.Children[0], path.Nodes[1]);
        }
    }
}
