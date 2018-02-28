using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform player;
    public Transform reciever;

    private bool playerIsOverlapping;
    private Transform myTransform;

    void Start()
    {
        myTransform = GetComponent<Transform>();
    }

	void Update()
    {
        if(playerIsOverlapping)
        {
            Vector3 portalToPlayer = player.position - myTransform.position;
            float dotProduct = Vector3.Dot(myTransform.forward, portalToPlayer);

            if(dotProduct < 0f)
            {
//                float rotationDifference = -Quaternion.Angle(myTransform.rotation, reciever.rotation);
                float rotationDifference = Vector3.SignedAngle(myTransform.forward, reciever.forward, Vector3.up);
                rotationDifference += 180;
                player.Rotate(Vector3.up, rotationDifference);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDifference, 0f) * portalToPlayer;
                player.position = reciever.position + positionOffset;

                playerIsOverlapping = false;
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerIsOverlapping = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerIsOverlapping = false;
        }
    }
}
