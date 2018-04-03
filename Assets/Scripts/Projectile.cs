using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float maxLifetime = 5.0f;
    public float speed = 5.0f;

    private Rigidbody myRigidbody;
    private Transform myTransform;

	void Start()
    {
        Destroy(gameObject, maxLifetime);
        myRigidbody = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();
	}

	void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + (speed * myTransform.forward * Time.deltaTime));
	}
}
