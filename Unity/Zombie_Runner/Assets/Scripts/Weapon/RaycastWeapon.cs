﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RaycastWeapon : MonoBehaviour
{
    [SerializeField] protected Camera FPCamera;
    [SerializeField] protected float range = 100f;
    [SerializeField] protected int damage = 35;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected GameObject hitEffect;
    [SerializeField] protected float timeBetweenShots = 0.1f;
    [SerializeField] protected bool isAuto = false;

    protected Ammo ammo = new Ammo();
    bool canShoot = true;

    void Awake()
    {
        CheckMainCamera();
    }

    private void CheckMainCamera()
    {
        if (!FPCamera)
        {
            FPCamera = Camera.main;
        }
    }

    void Update()
    {
        if (isAuto)
        {
            if (Input.GetKey(KeyCode.Mouse0) && Time.timeScale > 0f && canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.timeScale > 0f && canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        if (ammo.GetCurrentAmmo() > 0)
        {
            PlayMuzzleFlash();
            ProcessRaycast();
            ammo.ReduceCurrentAmmo();
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    public virtual void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void ProcessRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
        {
            CreatreHitImpact(hit);
            BaseStats targetStats = hit.transform.GetComponent<BaseStats>();
            if (targetStats)
            {
                targetStats.TakeDamage(damage);
            }
        }
    }

    private void CreatreHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.3f);
    }
}
