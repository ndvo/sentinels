using System;
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
    private GameObject _aim;
    [SerializeField] private bool inertia;

    public void Awake()
    {
        _aim = new GameObject();
    }

    private void FixedUpdate()
    {
        _setNewDirection();
        _positionAim();
        LookAtDirection();
        _deltaTimeSpeed = speed * Time.deltaTime;
        _move(transform, _deltaTimeSpeed);
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
        _aim.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _aim.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z,
            transform.rotation.w);
        _move(_aim.transform, speed);
    }

    private void LookAtDirection()
    {
        transform.LookAt(_aim.transform.position);
    }

}
