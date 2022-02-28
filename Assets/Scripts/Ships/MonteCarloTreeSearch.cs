using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ships
{
    public class TreeNode
    {
        public TreeNode(bool blocked, bool leaf, Vector3 position, TreeNode[] children = null)
        {
            Blocked = blocked;
            Leaf = leaf;
            Position = position;
            Children = children ?? new TreeNode[] { };
        }

        public bool Blocked;
        public bool Leaf;
        public Vector3 Position;
        public TreeNode[] Children;
    
        public override string ToString() => $"blocked {Blocked}, leaf {Leaf}, position {Position.x},{Position.y},{Position.z}, Children: {Children.Length}";
    }

    public class Path
    {
        public TreeNode Leaf;
        public readonly List<TreeNode> Nodes;
        public int Score;
        public float Distance;
        public bool Reached;
    
        public Path(int start, TreeNode leaf)
        {
            Score = start;
            Leaf = leaf;
            Distance = float.PositiveInfinity;
            Nodes = new List<TreeNode>();
            Reached = false;
        }
    
    }

    public class MonteCarloTreeSearch 
    {

        /// <summary>
        /// Returns the longest unobstructed path that is closer to the target.
        /// 
        /// Given a list TreeNode, returns a Path object describing the chosen path.
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="target"></param>
        /// <param name="path"></param>
        /// <param name="toleranceDistance"></param>
        /// <returns></returns>
        public Path ComputePath(TreeNode node, Vector3 target, float toleranceDistance = 0f)
        {
            var path = new Path(0, node);
            if (!node.Leaf && !node.Blocked && node.Children != null && node.Children.Length > 0)
            {
                // otherwise, work through the graph
                // choose among all children the path with the longest non-blocking nodes
                path = node.Children.Aggregate(path, (curr, other) =>
                {
                    var otherPath = ComputePath(other, target); // Invoke the children, the leaf will create the path
                    return _choosePath(curr, otherPath);
                });
            }
            _handlePath(path, node, target, toleranceDistance);
            return path;
        }

        private Path _choosePath(Path a, Path b)
        {
            Path chosen;
            if (a.Reached) return a; // this path reaches the target, return it
            if (b.Reached) return b; // this path reaches the target, return it
            if (a.Score == b.Score) // For equal unblocked steps, use the shortest distance
            {
                chosen = a.Distance <= b.Distance ? a : b;
            }
            else
            {
                chosen = a.Score > b.Score ? a : b; // prefer the longest unblocked path
            }
            return chosen;
        }

        private void _handlePath(Path path, TreeNode node, Vector3 target, float toleranceDistance)
        {
            // insert current node in path.
            path.Nodes.Insert(0, node);
            // add 1 to score if node is not blocked.
            path.Score = (node.Blocked ? 0 : 1) + path.Score;
            // distance is set on leaf.
            var lastNode = path.Nodes.ElementAt(path.Nodes.Count - 1);
            path.Distance = Vector3.Distance(lastNode.Position, target);
            // if current node is the target position, the path is flagged as reached.
            path.Reached = (Vector3.Distance(node.Position, target) <= toleranceDistance);
        }

    }
}