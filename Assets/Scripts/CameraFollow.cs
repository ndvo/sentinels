using Ships;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject;
    private OrbitalFlight targetFlight;

    void Start()
    {
        targetFlight = targetObject.GetComponent<OrbitalFlight>();
        targetFlight.GoToStartPosition(transform);
    }

    private void FixedUpdate()
    {
        targetFlight.MoveWithMe(transform);
    }
}
