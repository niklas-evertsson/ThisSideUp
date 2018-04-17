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

    // Clears the trail of the trail renderer, used by PortalTeleporter
    public void ClearTrail()
    {
        myTrail.Clear();
    }

	void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myTrail = GetComponent<TrailRenderer>();
        myTransform = GetComponent<Transform>();

        // Set properties from inspector
        myTrail.startColor = color;
        myTrail.endColor = color;

        // Set a time limit on the projectiles lifetime
        Destroy(gameObject, maxLifetime);
	}

	void FixedUpdate()
    {
        // Move the projectile forward
        myRigidbody.MovePosition(myRigidbody.position + (speed * myTransform.forward * Time.deltaTime));
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if(playerController != null)
            {
                // Reduce the health of the player
                playerController.health -= 1;
                Destroy(gameObject);
            }
        }
        // Only destroy if other is not a face and not a portal
        else if(!other.CompareTag("Face") && !other.CompareTag("Portal"))
        {
            Destroy(gameObject);
        }
    }
}
