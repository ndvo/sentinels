using UnityEngine;

public class CrossHairScript : MonoBehaviour
{
    private GameObject _mainCamera;
    private SpaceShip _spaceShip;
    private GameObject _target;


    private void Start()
    {
        _mainCamera = GameObject.Find("Main Camera");
    }

    private void Update()
    {
        if (_target is null || !_spaceShip.alive) UnsetTarget();
        else transform.position = _target.transform.position;
        transform.rotation = _mainCamera.transform.rotation;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
        _spaceShip = _target.transform.GetComponentInChildren<SpaceShip>();
        if (_spaceShip is {alive: true})
            gameObject.SetActive(true);
        else
            UnsetTarget();
    }

    public void UnsetTarget()
    {
        _target = null;
        gameObject.SetActive(false);
    }
}