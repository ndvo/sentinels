using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetObject;
    private Vector3 initialOffset = new Vector3(0f, 200f, 0f);

    void Start()
    {
        initialOffset = transform.position - targetObject.position;
    }

    private void FixedUpdate()
    {
        var cameraPosition = targetObject.position + initialOffset;
        transform.SetPositionAndRotation(cameraPosition, transform.rotation);
    }
}
