using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool inputEnabled = true;
    public float gravity = 20.0f;
    public float gravityAlignementSpeed = 20.0f;
    public float jumpSpeed = 10.0f;
    public float lookSpeed = 2.0f;
    public float moveSpeed = 20.0f;

    private const float minGroundNormalY = 0.65f;
    private const float shellRadius = 0.01f;
    private bool isGrounded;
    private float distance;
    private float lookY;
    private Camera myCamera;
    private RaycastHit[] hitBuffer;
    private Rigidbody myRigidbody;
    private Transform myTransform;
    private Vector3 desiredMove;
    private Vector3 desiredJump;
    private Vector3 gravityUp;

	void Start()
    {
        myCamera = GetComponentInChildren<Camera>();
        myRigidbody = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();
        gravityUp = myTransform.up;
	}

	void Update()
    {
        if(inputEnabled)
        {
            Look();
        }
	}

    void FixedUpdate()
    {
        if(isGrounded && inputEnabled)
        {
            GetInput();
        }
        isGrounded = false;

        ApplyGravity();

        MovePlayer(desiredMove * Time.deltaTime);
        MovePlayer(desiredJump * Time.deltaTime);
    }

    void ApplyGravity()
    {
        // Align player to gravity
        Quaternion gravityRotation = Quaternion.FromToRotation(myTransform.up, gravityUp) * myTransform.rotation;
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, gravityRotation, gravityAlignementSpeed * Time.deltaTime);

        // Apply gravity
        desiredJump -= gravity * gravityUp * Time.deltaTime;
    }

    void GetInput()
    {
        var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Prevents player from walking faster diagonally
        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }

        input *= moveSpeed;

        if(Input.GetButtonDown("Jump"))
        {
            input.y = jumpSpeed;
        }

        desiredJump = myTransform.up * input.y;
        desiredMove = (myTransform.right * input.x) + (myTransform.forward * input.z);
    }

    void Look()
    {
        // Mouse player turning
        float lookX = Input.GetAxis("Mouse X") + Input.GetAxis("Turning") * 10;
        myTransform.Rotate(0, lookX * lookSpeed, 0);

        // Mouse camera pitch
        lookY -= Input.GetAxis("Mouse Y") * lookSpeed;
        lookY = Mathf.Clamp(lookY, -90, 90);
        myCamera.transform.localRotation = Quaternion.Euler(lookY, 0, 0);
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

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Face"))
        {
            gravityUp = other.transform.up;
        }
    }
}
