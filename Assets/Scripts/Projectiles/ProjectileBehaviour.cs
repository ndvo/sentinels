using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectiveBehaviour : MonoBehaviour
{

    public float duration;
    public float power;
    private bool _fadeOut = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            _fadeOut = true;
        }

    }
}
