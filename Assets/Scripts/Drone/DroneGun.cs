using System;
using UnityEngine;
[RequireComponent(typeof(DroneBase))]

public class DroneGun : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float bulletSpeed = 25f;
    [SerializeField] private float fireCooldown = 0.3f;

    private DroneBase drone;
    private Collider[] ownColliders;
    private float nextFireTime;

    private void Awake()
    {
        drone = GetComponent<DroneBase>();
        ownColliders = GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        if(!drone.IsControlled) return;
        if(!DroneInput.FireDown || Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireCooldown;
        Fire();
    }
    private void Fire()
    {
        if(bulletPrefab == null || muzzle == null) return;

        Bullet bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);

        var bulletCol = bullet.GetComponent<Collider>();
        if(bulletCol != null)
        {
            foreach(var c in ownColliders) Physics.IgnoreCollision(bulletCol, c);
        }
        bullet.Init(muzzle.forward, bulletSpeed, transform.position);
    }
}
