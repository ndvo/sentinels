using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairScript : MonoBehaviour
{
    private GameObject _mainCamera;
    private GameObject _target;
    private SpaceShip _spaceShip;

    
    void Start()
    {
        _mainCamera = GameObject.Find("Main Camera");
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
        _spaceShip = _target.transform.GetComponentInChildren<SpaceShip>();
        if (_spaceShip.alive)
            gameObject.SetActive(true);
        else
            UnsetTarget();
    }

    public void UnsetTarget()
    {
        _target = null;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (_target is null || !_spaceShip.alive) UnsetTarget();
        else transform.position = _target.transform.position;
        transform.rotation = _mainCamera.transform.rotation;
    }
}
