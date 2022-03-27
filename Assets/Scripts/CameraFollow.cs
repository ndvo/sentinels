using Ships;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject;
    private ShipFlight _targetFlight;

    void Start()
    {
        _targetFlight = targetObject.GetComponent<ShipFlight>();
        _targetFlight.GoToStartPosition(transform);
    }

    private void Update()
    {
        _targetFlight.MoveWithMe(transform);
    }
}
