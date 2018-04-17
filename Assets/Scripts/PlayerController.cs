using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Teleportable
{
    public static bool inputEnabled = true;
    [HideInInspector]
    public Vector3 gravityUp;

    [Header("Player")]
    public float gravity = 20.0f;
    public float gravityAlignementSpeed = 20.0f;
    public float jumpSpeed = 10.0f;
    public float lookSmoothing = 0.25f;
    public float lookSpeed = 2.0f;
    public float moveSpeed = 20.0f;
    public int health = 3;
    public int playerNumber = 1;
    public Color playerColor = Color.white;

    [Header("Gun")]
    public float fireRate = 0.25f;
    public float gunRange = 100.0f;
    public Transform gunEnd;

    [Header("Projectile")]
    public float projectileMaxLifetime = 5.0f;
    public float projectileSpeed = 100.0f;
    public Projectile projectile;

    private const float minGroundNormalY = 0.65f;
    private const float shellRadius = 0.01f;
    private bool isGrounded;
    private float distance;
    private float lookY;
    private float nextFire;
    private float smoothLookX;
    private float smoothLookY;
    private string fireInput;
    private string horizontalInput;
    private string jumpInput;
    private string verticalInput;
    private string xLookInput;
    private string yLookInput;
    private Camera myCamera;
    private MeshRenderer[] myMeshRenderers;
    private RaycastHit[] hitBuffer;
    private Rigidbody myRigidbody;
    private Transform myTransform;
    private Vector3 desiredMove;
    private Vector3 desiredJump;

	void Start()
    {
        myCamera = GetComponentInChildren<Camera>();
        myMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        myRigidbody = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();
        gravityUp = myTransform.up;

        fireInput       = "Fire"       + playerNumber;
        horizontalInput = "Horizontal" + playerNumber;
        jumpInput       = "Jump"       + playerNumber;
        verticalInput   = "Vertical"   + playerNumber;
        xLookInput      = "LookX"      + playerNumber;
        yLookInput      = "LookY"      + playerNumber;
	}

	void Update()
    {
        // Turn off script and renderer if health is zero
        if(health <= 0)
        {
            enabled = false;
            foreach(MeshRenderer r in myMeshRenderers)
            {
                r.enabled = false;
            }
        }

        if(inputEnabled)
        {
            Look();

            bool shooting = Input.GetButton(fireInput) || Input.GetAxis(fireInput) > 0;
            if(shooting && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Shoot();
            }
        }
	}

    void FixedUpdate()
    {
        AlignToGravity();

        if(isGrounded)
        {
            if(inputEnabled)
            {
                GetInput();
            }
        }
        else
        {
            // Apply gravity
            desiredJump -= gravity * gravityUp * Time.deltaTime;
        }
        isGrounded = false;

        MovePlayer(desiredJump * Time.deltaTime);
        MovePlayer(desiredMove * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Face"))
        {
            gravityUp = other.transform.up;
        }
    }

    void AlignToGravity()
    {
        // Align player to gravity
        Quaternion gravityRotation = Quaternion.FromToRotation(myTransform.up, gravityUp) * myTransform.rotation;
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, gravityRotation, gravityAlignementSpeed * Time.deltaTime);

    }

    void GetInput()
    {
        var input = new Vector3(Input.GetAxis(horizontalInput), 0, Input.GetAxis(verticalInput));

        // Prevents player from walking faster diagonally
        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }
        input *= moveSpeed;

        if(Input.GetButtonDown(jumpInput))
        {
            input.y = jumpSpeed;
        }

        desiredJump = gravityUp * input.y;
        desiredMove = Vector3.ProjectOnPlane((myTransform.right * input.x) + (myTransform.forward * input.z), gravityUp);
    }

    void Look()
    {
        // Mouse player turning
        float lookX = (Input.GetAxis(xLookInput) + Input.GetAxis("Turning")) * lookSpeed;
        smoothLookX = Mathf.Lerp(smoothLookX, lookX, lookSmoothing);
        myTransform.Rotate(0, smoothLookX, 0);

        // Mouse camera pitch
        lookY -= Input.GetAxis(yLookInput) * lookSpeed;
        smoothLookY = Mathf.Lerp(smoothLookY, lookY, lookSmoothing);
        smoothLookY = Mathf.Clamp(smoothLookY, -90, 90);
        myCamera.transform.localRotation = Quaternion.Euler(smoothLookY, 0, 0);
    }

    void MovePlayer(Vector3 velocity)
    {
        distance = velocity.magnitude;
        hitBuffer = myRigidbody.SweepTestAll(velocity, distance + shellRadius, QueryTriggerInteraction.Ignore);
        for(int i = 0; i < hitBuffer.Length; i++)
        {
            Vector3 currentNormal = hitBuffer[i].normal;
            isGrounded |= Vector3.Dot(gravityUp, currentNormal) > minGroundNormalY;

            float projection = Vector3.Dot(velocity, currentNormal);
            if(projection < -0.01f)
            {
                velocity -= projection * currentNormal;
            }

            float modifiedDistance = hitBuffer[i].distance - shellRadius;
            if(modifiedDistance < distance)
            {
                distance = modifiedDistance;
            }
        }

        // Move player
        myRigidbody.position += velocity.normalized * distance;
    }

    void Shoot()
    {
        RaycastHit hit;
        Vector3 target;
        // Check what the player is aiming at
        if(Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, gunRange))
        {
            target = hit.point;
        }
        else
        {
            target = myCamera.transform.position + (myCamera.transform.forward * gunRange);
        }
        // Aim gun where player looks
        gunEnd.LookAt(target);

        // Fire projectile and set its properties
        Projectile clone = Instantiate(projectile, gunEnd.position, gunEnd.rotation);
        clone.color = playerColor;
        clone.maxLifetime = projectileMaxLifetime;
        clone.speed = projectileSpeed;
    }
}
