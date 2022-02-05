using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class OrbitalFlight : MonoBehaviour
{
    
    public float speed;
    private int _verticalMovement = 0;
    private int _horizontalMovement = 0;
    private float _deltaTimeSpeed = 0;
    [SerializeField] private bool inertia;

    private void FixedUpdate()
    {
        _deltaTimeSpeed = speed * Time.deltaTime;
        SetHorizontalMovement();
        SetVerticalMovement();
        transform.RotateAround(
            new Vector3(0f, 0f, 0f),
            Vector3.left,
            _deltaTimeSpeed *  _verticalMovement
            );
        transform.RotateAround(
            new Vector3(0f, 0f, 0f),
            Vector3.forward,
            _deltaTimeSpeed * _horizontalMovement
            );
        LookAtDirection();
    }

    private void SetHorizontalMovement()
    {
        if (Input.GetKey("left"))
            _horizontalMovement = 1;
        if (Input.GetKey("right"))
            _horizontalMovement = -1;
        if (Input.GetKey("up") || Input.GetKey("down"))
            _horizontalMovement = 0;
    }

    private void SetVerticalMovement()
    {
        if (Input.GetKey("up"))
            _verticalMovement = -1;
        if (Input.GetKey("down"))
            _verticalMovement = 1;
        if (Input.GetKey("right") || Input.GetKey("left"))
            _verticalMovement = 0;
    }

    private void LookAtDirection()
    {
        if (Input.GetKey("up"))
            transform.LookAt(transform.position + new Vector3(0f, 0f, 1000f));
        if (Input.GetKey("down"))
            transform.LookAt(transform.position + new Vector3(0f, 0f, -1000f));
        if (Input.GetKey("right"))
            transform.LookAt(transform.position + new Vector3(1000f, 0f, 0f));
        if (Input.GetKey("left"))
            transform.LookAt(transform.position + new Vector3(-1000f, 0f, 0f));
    }
}
