using UnityEngine;

/// <summary>
/// Ship Container is the game object that is parent to all instantiated enemy vessels.
/// </summary>
public class ShipContainerBehaviour : MonoBehaviour
{
    private void Update()
    {
        _removeInactiveShips();
        
    }

    /// <summary>
    /// Remove enemy vessels five seconds after they are inactive
    ///
    /// This allows the enemy headquarters to instantiate new ships.
    /// </summary>
    private void _removeInactiveShips()
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf) continue;
            Destroy(t.gameObject, 5);
        }
    }
}
