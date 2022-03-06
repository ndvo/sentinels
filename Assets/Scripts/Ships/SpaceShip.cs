using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class SpaceShip : MonoBehaviour
{
    public float energyLevel = 1000;
    private ParticleSystem _explosionVFX;
    private ParticleSystem _explosionDestroyVFX;
    private AudioSource _explosionAudio;
    private GameObject _shield;
    private bool _hasShield = false;
    public bool alive = true;

    private void Start()
    {
        _explosionVFX = transform.Find("Explosion").GetComponent<ParticleSystem>();
        _explosionAudio = GetComponent<AudioSource>();
        _explosionDestroyVFX = transform.parent.transform.Find("ExplosionDestroy").GetComponent<ParticleSystem>();
        _explosionDestroyVFX = _explosionDestroyVFX != null ? _explosionDestroyVFX : _explosionVFX;
        _shield = transform.Find("Shield").gameObject;
        if (_shield != null) _hasShield = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        CheckCollisionWithSpaceStation(other);
        CheckCollisionWithSpaceShip(other);
    }

    private void CheckCollisionWithSpaceStation(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SpaceStation"))
        {
            TakeDamage(10);
        }
    }

    private void CheckCollisionWithSpaceShip(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Ship") &&
            other.gameObject.layer != LayerMask.NameToLayer("Sentinel")) return;
        TakeDamage(10);
    }

    private void FixedUpdate()
    {
        if (_hasShield) _shield.SetActive(energyLevel >= 900);
    }

    public float TakeDamage(float damage)
    {

        energyLevel -= damage;
        _explode();
        return energyLevel;
    }

    private void _explode()
    {
        _explosionAudio.Play();
        var explosion = _explosionVFX;
        if (energyLevel <= 0)
        {
            alive = false;
            explosion = _explosionDestroyVFX;
            Invoke(nameof(SetInactive), 0.3f);
            Destroy(transform.parent.gameObject, 5);
        }

        explosion.Clear();
        explosion.Stop();
        explosion.Play();
    }

    private void SetInactive()
    {
        transform.gameObject.SetActive(false);
    }





}
