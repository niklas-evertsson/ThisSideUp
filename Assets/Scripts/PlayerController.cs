using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float gravity = 20.0F;
    public float jumpSpeed = 8.0F;
    public float lookSpeed = 6.0F;
    public float moveSpeed = 6.0F;

    private Camera playerCamera;
    private CharacterController controller;
    private float cameraPitch;
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
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);

        // Mouse camera pitch
        cameraPitch += Input.GetAxis("Mouse Y") * -lookSpeed;
        cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

        if (controller.isGrounded)
        {
            // Get player input for movement
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move player
        controller.Move(moveDirection * Time.deltaTime);
    }
}
