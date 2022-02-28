using System.Collections.Generic;
using System.Linq;
using Ships;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;

public class SensorBehaviour : MonoBehaviour
{
    public TreeNode treeNode;
    public GameObject sensorTemplate;
    public int level;
    private int currentLevel;
    private SensorBehaviour parentSensor;
    public GameObject ship;
    private OrbitalFlight shipFlight;
    private MeshRenderer box;
    private readonly List<SensorBehaviour> _childSensors = new List<SensorBehaviour> ();
    private readonly List<GameObject> childNodes = new List<GameObject>();
    private Position direction = Direction.North;

    private void Awake()
    {
        treeNode = new TreeNode(false, false, Vector3.zero, new TreeNode[] { });
    }

    public void Start()
    {
        box = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        parentSensor = transform.parent.gameObject.GetComponent<SensorBehaviour>();
        ResetLevel();
        ResetTreeNode();
        ResetChildObjects();
        ResetChildren();
    }

    private void ResetTreeNode()
    {
        treeNode.Blocked = false;
        treeNode.Leaf = level - currentLevel == 0;
        treeNode.Position = transform.position;
    }

    private void ResetLevel()
    {
        if (parentSensor != null)
        {
            currentLevel = parentSensor.currentLevel + 1;
            ship = transform.parent.gameObject.GetComponent<SensorBehaviour>().ship;
        }
        else
        {
            ship = transform.parent.gameObject;
            shipFlight = ship.GetComponent<OrbitalFlight>();
        }
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
        if (other.gameObject.layer == LayerMask.NameToLayer("SpaceStation"))
        {
            treeNode.Blocked = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SpaceStation"))
        {
            treeNode.Blocked = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SpaceStation"))
        {
            treeNode.Blocked = false;
        }
    }
    
    private Position[] ChildrenDirections()
    {
        if (direction == Direction.North)
            return new Position[]
            {
                new Position(0, 1),
                new Position(1, 0),
                new Position(-1, 0)
            };
        if (direction == Direction.East)
        {
            return new Position[]
            {
                new Position(-1, 0),
                new Position(0, 1),
                new Position(0, -1)
            };
        }
        if (direction == Direction.West)
            return new Position[]
            {
                new Position(1, 0),
                new Position(0, 1),
                new Position(0, -1)
            };
        return new Position[] { };
    }
    
    private void CreateChild(Position dir)
    {
        var child = Object.Instantiate(gameObject, transform);
        var sensor = child.GetComponent<SensorBehaviour>();
        sensor.direction = dir;
    }

    private void ResetChildren()
    {
        if (level - currentLevel <= 0) return;
        CreateChildren();
        if (currentLevel == 0)
        {
            direction = Direction.East;
            CreateChildren();
            direction = Direction.West;
            CreateChildren();
            direction = Direction.North;
        }
    }

    private void CreateChildren()
    {
        var dirs = ChildrenDirections();
        for (int i = 0; i < dirs.Length; i++)
        {
            var child = Object.Instantiate(gameObject, transform);
            var sensor = child.GetComponent<SensorBehaviour>();
            _childSensors.Add(sensor);
            sensor.direction = i == 0 ? direction : new Position(0, 0);
            sensor.Move(dirs[i].x * 2, dirs[i].y * 2);
        }
        treeNode.Children = (from i in _childSensors select i.treeNode).ToArray();
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

    public void ShowIfInList(IEnumerable<TreeNode> list)
    {
        box.material.color = list.Contains(treeNode) ? Color.white : Color.black;
        foreach (var sensor in _childSensors)
        {
            sensor.ShowIfInList(list);
        }
    }
}
