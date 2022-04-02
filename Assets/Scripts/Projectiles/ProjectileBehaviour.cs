using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behaviour of the projectiles (mines) launched by enemies.
/// </summary>
public class ProjectileBehaviour : MonoBehaviour
{

    public float duration;
    public float power;
    private bool _fadeOut = false;
    private Material _material;

    private void Start()
    {
        _material = transform.GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            _fadeOut = true;
        }

        if (_fadeOut)
        {
            var oldColor = _material.color;
            var newColor = Mathf.Max(0, oldColor.a - 0.05f * Time.deltaTime);
            _material.color = new Color(oldColor.r, oldColor.g, oldColor.b, newColor);
            if (_material.color.a <= 0f)
            {
                Destroy(this);
            }
        }
    }
    
}
