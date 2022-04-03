using Ships;
using UnityEngine;

/// <summary>
///     Controls the behaviour of the camera.
///     Camera should follow an object with a ShipFlight component.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject;
    private ShipFlight _targetFlight;

    private void Start()
    {
        _targetFlight = targetObject.GetComponent<ShipFlight>();
        _targetFlight.GoToStartPosition(transform);
    }

    private void FixedUpdate()
    {
        _targetFlight.MoveWithMe(transform);
    }
}