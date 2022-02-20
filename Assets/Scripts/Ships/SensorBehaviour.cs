using System.Linq;
using Ships;
using UnityEngine;
using Object = UnityEngine.Object;

public class SensorBehaviour : MonoBehaviour
{
    private TreeNode treeNode;
    public GameObject sensorTemplate;
    public int level;
    public bool root = false;
    private SensorBehaviour parentSensor;

    public void Start()
    {
        parentSensor = transform.parent.gameObject.GetComponent<SensorBehaviour>();
        ResetLevel();
        ResetTreeNode();
        ResetChildObjects();
        CreateChildren();
    }

    private void ResetTreeNode()
    {
        treeNode.Blocked = false;
        treeNode.Leaf = level == 0;
        treeNode.Position = transform.position;
    }

    private void ResetLevel()
    {
        level = (parentSensor != null) ? parentSensor.level - 1 : level;
    }

    private void ResetChildObjects()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.gameObject.GetComponent<SensorBehaviour>() != null)
                Destroy(child.gameObject);
        }
    }


    public TreeNode GetTreeNode()
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

    private void CreateChildren()
    {
        if (level <= 0) return;
        var forward = Object.Instantiate(gameObject, transform);
        var left = Object.Instantiate(gameObject, transform);
        var right = Object.Instantiate(gameObject, transform);
        var newNodes = new[] {forward, left, right};
        // Position child sensors
        forward.GetComponent<SensorBehaviour>().Move(0, 2);
        left.GetComponent<SensorBehaviour>().Move(-2, 0);
        right.GetComponent<SensorBehaviour>().Move(2, 0);
        treeNode.Children = (from n in newNodes 
            select n.GetComponent<SensorBehaviour>().treeNode).ToArray();
    }

    public void Move(int directionY, int directionZ)
    {
        //transform.LookAt(transform.position + new Vector3(directionY, 0, directionZ));
        if (directionZ != 0) transform.RotateAround(
            Vector3.zero, 
            Vector3.right,
            directionZ
        );
        if (directionY != 0) transform.RotateAround(
            Vector3.zero, 
            Vector3.forward,
            directionY
        );
    }
}
