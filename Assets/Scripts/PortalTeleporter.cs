using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform player;
    public Transform myPortal;
    public Transform otherPortal;
    public Transform reciever;

    private bool playerIsOverlapping;
    private Transform myTransform;

    void Start()
    {
        myTransform = GetComponent<Transform>();
    }

	void Update()
    {
        // Check if player is inside the collider
        if(playerIsOverlapping)
        {
            // Player position relative to this teleporter
            Vector3 localPlayerPosition = myTransform.InverseTransformPoint(player.position);
            // If player is behind the teleporter
            if(localPlayerPosition.z < 0)
            {
                // Invert X so that the left/right position in the portal matches after rotation
                localPlayerPosition.x = -localPlayerPosition.x;

                // Player forward direction relative to this portal
                Vector3 localPlayerForward = myPortal.InverseTransformDirection(player.forward);

                // Set player position relative to reciever
                player.position = reciever.TransformPoint(localPlayerPosition);
                // Set player position relative to other Portal
                player.rotation = Quaternion.LookRotation(otherPortal.TransformDirection(localPlayerForward), otherPortal.up);

                playerIsOverlapping = false;
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Player is inside the collider
            playerIsOverlapping = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Player has left the collider
            playerIsOverlapping = false;
        }
    }
}
