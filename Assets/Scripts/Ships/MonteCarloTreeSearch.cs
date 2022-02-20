using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ships
{
    public struct TreeNode
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
        public Path ComputePath(TreeNode node, Vector3 target, Path path = null, float toleranceDistance = 0f)
        {
            if (node.Leaf || node.Blocked)
            {
                path = new Path(0, node);
                _handlePath(path, node, target, toleranceDistance);
                // if leaf, no more work to do
                return path;
            }
            // otherwise, work through the graph
            // choose among all children the path with the longest non-blocking nodes
            var chosen = node.Children.Aggregate(path, (curr, child) =>
            {
                path = ComputePath(child, target); // Invoke the children, the leaf will create the path
                if (curr is null) return path; // If we are the first to evaluate (last of the stack) return path
                if (curr.Reached) return curr; // this path reaches the target, return it
                if (path.Reached) return path; // this path reaches the target, return it
                if (curr.Score == path.Score) // For equal unblocked steps, use the shortest distance
                {
                    return curr.Distance <= path.Distance ? curr : path;
                }
                return (curr.Score > path.Score) ? curr : path; // prefer the longest unblocked path
            });
            _handlePath(chosen, node, target, toleranceDistance);
            return chosen;
        }

        private void _handlePath(Path path, TreeNode node, Vector3 target, float toleranceDistance)
        {
            // insert current node in path.
            path.Nodes.Insert(0, node);
            // add 1 to score if node is not blocked.
            path.Score = (node.Blocked ? 0 : 1) + path.Score;
            // distance is set on leaf.
            path.Distance = node.Leaf ? UnityEngine.Vector3.Distance(node.Position, target) : float.PositiveInfinity;
            // if current node is the target position, the path is flagged as reached.
            path.Reached = (Vector3.Distance(node.Position, target) <= toleranceDistance);
        }

    }
}