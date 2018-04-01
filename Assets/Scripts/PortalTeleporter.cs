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
                // Invert X and Z to match other portal after rotation
                localPlayerPosition.x = -localPlayerPosition.x;
                localPlayerPosition.z = -localPlayerPosition.z;

                // Player forward direction relative to this portal
                Vector3 localPlayerForward = myPortal.InverseTransformDirection(player.forward);

                // Set player position and rotation relative to other portal
                player.position = reciever.TransformPoint(localPlayerPosition);
                player.rotation = Quaternion.LookRotation(otherPortal.TransformDirection(localPlayerForward), otherPortal.up);

                playerIsOverlapping = false;
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        // Player is inside the collider
        playerIsOverlapping |= other.CompareTag("Player");
    }

    void OnTriggerExit(Collider other)
    {
        // Player has left the collider
        playerIsOverlapping &= !other.CompareTag("Player");
    }
}
