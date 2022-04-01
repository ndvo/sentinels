using UnityEngine;

public class ShipContainerBehaviour : MonoBehaviour
{
    private void Update()
    {
        _removeInactiveShips();
        
    }

    private void _removeInactiveShips()
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf) continue;
            Destroy(t.gameObject, 5);
        }
    }
}
