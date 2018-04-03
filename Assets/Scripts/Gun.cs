using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fireRate = 0.25f;
    public float gunRange = 100.0f;
    public float projectileSpeed = 10.0f;
    public float projectileMaxLifetime = 5.0f;
    public Projectile projectile;
    public Transform gunEnd;

    private float nextFire;
    private Camera playerCamera;
    private Vector3 target;

    void Start()
    {
        playerCamera = GetComponentInParent<Camera>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            RaycastHit hit;
            if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, gunRange, Physics.IgnoreRaycastLayer))
            {
                target = hit.point;
            }
            else
            {
                target = playerCamera.transform.position + (playerCamera.transform.forward * gunRange);
            }
            gunEnd.LookAt(target);

            Projectile clone = Instantiate(projectile, gunEnd.position, gunEnd.rotation);
            clone.maxLifetime = projectileMaxLifetime;
            clone.speed = projectileSpeed;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(target, 0.5f);
    }
}
