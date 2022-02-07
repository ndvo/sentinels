using System.Linq;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public interface TreeNode
{
    public TreeNode[] GetChildren();
    public bool IsBlocked();
    public bool IsLeaf();
    public Vector3 GetPosition();
}

public class MonteCarloTreeSearch 
{

    /// <summary>
    /// Returns the longest unobstructed path that is closer to the target.
    /// </summary>
    /// <param name="paths"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private TreeNode _search(TreeNode[] paths, Vector3 target)
    {
        return paths.Aggregate(paths[0], (a, b) =>
        {
            // Get the length of the unobstructed path.
            var scoreA = ComputeScore(a);
            var scoreB = ComputeScore(b);
            return (scoreA == scoreB)
                ? a.GetPosition() == _shortestPath(new []{a.GetPosition(), b.GetPosition()}, target) ? a : b
                : ComputeScore(a) < ComputeScore(b) ? b : a;
        });
    }

    /// <summary>
    /// Computes the score for a given node.
    ///
    /// It sums the value of all child nodes, then adds the value of the current node.
    /// The value of the current node is 1 if it is not blocked and zero otherwise.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private int ComputeScore(TreeNode node)
    {
        var myValue = node.IsBlocked() ? 0 : 1;
        if (node.IsLeaf()) return myValue;
        return myValue + node.GetChildren().Aggregate(0, (total, n) => ComputeScore(n));
    }

    private Vector3 _shortestPath(Vector3[] steps, Vector3 target)
    {
        var minDistance = 999999999f;
        var selected = steps[0];
        foreach (var step in steps)
        {
            var distance = _distance(step, target);
            if (distance < minDistance)
            {
                selected = step;
                minDistance = distance;
            }
        }
        return selected;
    }

    private float _distance(Vector3 pointA, Vector3 pointB)
    {
        return Vector3.Distance(pointB, pointA);
    }
}
