using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TrailRenderer))]
public class Projectile : Teleportable
{
    public float maxLifetime = 5.0f;
    public float speed = 5.0f;
    public Color color = Color.white;

    private Rigidbody myRigidbody;
    private TrailRenderer myTrail;
    private Transform myTransform;

    public void ClearTrail()
    {
        myTrail.Clear();
    }

	void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myTrail = GetComponent<TrailRenderer>();
        myTransform = GetComponent<Transform>();

        myTrail.startColor = color;
        myTrail.endColor = color;

        Destroy(gameObject, maxLifetime);
	}

	void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + (speed * myTransform.forward * Time.deltaTime));
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if(playerController != null)
            {
                playerController.health -= 1;
            }
        }
        else if(!other.CompareTag("Face") && !other.CompareTag("Portal"))
        {
            Destroy(gameObject);
        }
    }
}
