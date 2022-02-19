using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject;
    private OrbitalFlight targetOrbitalFlight;

    void Start()
    {
        targetOrbitalFlight = targetObject.GetComponent<OrbitalFlight>();
    }

    private void FixedUpdate()
    {
        targetOrbitalFlight.MoveWithMe(this.transform);
    }
}
