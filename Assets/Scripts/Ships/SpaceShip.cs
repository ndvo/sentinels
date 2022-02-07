using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class SpaceShip : MonoBehaviour
{
    public float energyLevel = 1000;
    private ParticleSystem _explosionPS;
    private AudioSource _explosionAudio;
    private GameObject _shield;
    
    private void Start()
    {
        _explosionPS = GameObject.Find("Explosion").GetComponent<ParticleSystem>();
        _explosionAudio = GetComponent<AudioSource>();
        _shield = GameObject.Find("Shield");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SpaceStation"))
        {
            TakeDamage(1);
            _explode();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        _shield.SetActive(energyLevel >= 900);
    }

    public float TakeDamage(float damage)
    {
        energyLevel -= damage;
        return damage;
    }

    private void _explode() {
        _explosionAudio.Play();
        _explosionPS.Play();
    }

}
