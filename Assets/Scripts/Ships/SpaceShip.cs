using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class SpaceShip : MonoBehaviour
{
    private ParticleSystem _explosionPS;
    private AudioSource _explosionAudio;
    
    private void Start()
    {
        _explosionPS = GameObject.Find("Explosion").GetComponent<ParticleSystem>();
        _explosionAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SpaceStation"))
        {
            _explode();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _explode() {
        _explosionAudio.Play();
        _explosionPS.Play();
    }

}
