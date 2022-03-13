using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class SpaceShip : MonoBehaviour
{
    public float energyLevel = 1000;
    private ParticleSystem _explosionVFX;
    private ParticleSystem _explosionDestroyVFX;
    private ParticleSystem _explosionSmokeVFX;
    private AudioSource _explosionAudio;
    private GameObject _shield;
    private bool _hasShield = false;
    public bool alive = true;

    private void Start()
    {
        var parentTransform = transform.parent.transform;
        _explosionVFX = transform.Find("Explosion").GetComponent<ParticleSystem>();
        _explosionAudio = parentTransform.Find("jukebox")?.GetComponent<AudioSource>();
        var explosionDestroy = parentTransform.Find("ExplosionDestroy");
        if (explosionDestroy is { }) _explosionDestroyVFX = explosionDestroy.GetComponent<ParticleSystem>();
        _explosionDestroyVFX = !(_explosionDestroyVFX is null) ? _explosionDestroyVFX : _explosionVFX;
        var explosionSmoke = parentTransform.Find("ExplosionSmoke");
        if (explosionSmoke is { }) _explosionSmokeVFX = explosionSmoke.GetComponent<ParticleSystem>();
        _explosionSmokeVFX = !(_explosionSmokeVFX is null) ? _explosionSmokeVFX : _explosionVFX;
        var shield = transform.Find("Shield");
        if (shield is { })
        {
            _shield = shield.gameObject;
            _hasShield = true;
        }
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
        if (energyLevel <= 0)
        {
            alive = false;
            if (_explosionDestroyVFX is null) {
                Debug.Log("No destroy explosion found");
                return; // the ship may have been destroyed
            }
            _explosionDestroyVFX.Clear();
            _explosionDestroyVFX.Stop();
            _explosionSmokeVFX.Clear();
            _explosionSmokeVFX.Stop();
            _explosionDestroyVFX.Play();
            _explosionSmokeVFX.Play();
            transform.gameObject.SetActive(false);
            Invoke(nameof(SetInactive), 1.2f);
        }
        else
        {
            if (_explosionVFX is null) return; // the ship may have been destroyed
            _explosionVFX.Clear();
            _explosionVFX.Stop();
            _explosionVFX.Play();
        }
    }

    private void SetInactive()
    {
        transform.parent.gameObject.SetActive(false);
    }





}
