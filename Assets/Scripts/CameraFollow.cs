using Ships;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject;
    private SentinelFlight sentinelFlight;

    void Start()
    {
        sentinelFlight = targetObject.GetComponent<SentinelFlight>();
        sentinelFlight.GoToStartPosition(transform);
    }

    private void FixedUpdate()
    {
        sentinelFlight.MoveWithMe(transform);
    }
}
