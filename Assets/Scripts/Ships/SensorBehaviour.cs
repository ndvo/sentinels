using UnityEngine;

public class SensorBehaviour : MonoBehaviour
{
    private TreeNode treeNode;

    private bool blocked;
    private bool leaf;

    // Start is called before the first frame update
    void Start()
    {
        blocked = false;
    }

    public void SetLeaf()
    {
        this.treeNode.Leaf = true;
    }

    public void SetTreeNode(TreeNode treeNode)
    {
        this.treeNode = treeNode;
    }

    public TreeNode GetTreeNode(TreeNode treeNode)
    {
        return this.treeNode;
    }

    public void OnTriggerEnter(Collider other)
    {
        treeNode.Blocked = true;
    }

    public void OnTriggerStay(Collider other)
    {
        treeNode.Blocked = true;
    }

    public void OnTriggerExit(Collider other)
    {
        treeNode.Blocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
