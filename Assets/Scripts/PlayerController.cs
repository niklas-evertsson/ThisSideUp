using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float gravity = 20.0f;
    public float gravityAlignementSpeed = 2.0f;
    public float jumpSpeed = 10.0f;
    public float lookSpeed = 5.0f;
    public float moveSpeed = 20.0f;

    private Camera playerCamera;
    private CharacterController controller;
    private float cameraPitch;
    private Vector3 gravityUp = Vector3.up;
    private Vector3 lookDirection;
    private Vector3 moveDirection = Vector3.zero;

	void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();
	}
    void Update()
    {
        // Mouse player turning
        transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        // Mouse camera pitch
        cameraPitch += Input.GetAxis("Mouse Y") * -lookSpeed;
        cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

//        if (controller.isGrounded)
        if(controller.collisionFlags != CollisionFlags.None)
        {
            // Get player input for movement
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;
            if (Input.GetButton("Jump"))
            {
                moveDirection += gravityUp * jumpSpeed;
            }
        }

        // Align player to gravity
        Quaternion gravityRotation = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, gravityRotation, gravityAlignementSpeed * Time.deltaTime);

        // Apply gravity
        moveDirection -= gravityUp * gravity * Time.deltaTime;

        // Move player
        controller.Move(moveDirection * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Face"))
        {
            gravityUp = other.transform.up;
        }
    }
}
