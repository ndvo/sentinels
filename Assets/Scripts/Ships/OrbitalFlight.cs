using UnityEngine;
using Utils;
using Time = UnityEngine.Time;
using Vector3 = UnityEngine.Vector3;

public class OrbitalFlight : MonoBehaviour
{

    public float speed;
    private Position _direction = new Position(0, 0);
    private Position _previousDirection = new Position(0, 0);
    private float _deltaTimeSpeed = 0;
    // aim is used to set the direction the ship is looking to.
    // the idea is to have an object slightly ahead that moves in the same manner (transform and rotate)
    // using the aim to guide the lookAt function allow us to get rid of bugs due to the orbital navigation.
    private GameObject aim;
    [SerializeField] private bool inertia;
    // The SpaceStationBuilder is responsible for creating the board as is capable of noticing if the ship has left the board.
    private SpaceStationBuilder spaceStationBuilder;
    private bool offBoard;
    public float warpMultiplier;

    public void Awake()
    {
        aim = new GameObject();
        spaceStationBuilder = GameObject.Find("SpaceStationBuilder").GetComponent<SpaceStationBuilder>();
    }

    private void FixedUpdate()
    {
        offBoard = spaceStationBuilder.IsOffBoard(transform.position);
        _orbitalFlight();
    }

    private void _orbitalFlight()
    {
        _deltaTimeSpeed = speed * Time.deltaTime;
        if (offBoard) _deltaTimeSpeed *= warpMultiplier;
        if (!offBoard) _setNewDirection();
        _positionAim();
        LookAtDirection();
        _move(transform, _deltaTimeSpeed);
    }

    public void MoveWithMe(Transform t)
    {
        _move(t, _deltaTimeSpeed);
    }

    private void _move(Transform t, float moveSpeed)
    {
        t.RotateAround(
            Vector3.zero, 
            Vector3.left,
            moveSpeed * _direction.y
        );
        t.RotateAround(
            Vector3.zero, 
            Vector3.forward,
            moveSpeed * _direction.x
        );
    }

    private void _setNewDirection()
    {
        _previousDirection = new Position(_direction.x, _direction.y);
        if (Input.GetKey("left"))
            _direction = Direction.East;
        if (Input.GetKey("right"))
            _direction = Direction.West;
        if (Input.GetKey("down"))
            _direction = Direction.South;
        if (Input.GetKey("up"))
            _direction = Direction.North;
    }

    private void _positionAim()
    {
        var position = transform.position;
        var rotation = transform.rotation;
        aim.transform.position = new Vector3(position.x, position.y, position.z);
        aim.transform.rotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
        _move(aim.transform, speed);
    }

    private void LookAtDirection()
    {
        transform.LookAt(aim.transform.position);
    }
}
