using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform player;
    public Transform myPortal;
    public Transform otherPortal;
    public Transform reciever;

    private List<string> myTeleportables;
    private List<Transform> objectsToTeleport = new List<Transform>();
    private Transform myTransform;

    void Start()
    {
        myTeleportables = FindObjectOfType<GameManager>().teleportables;
        myTransform = GetComponent<Transform>();
    }

	void Update()
    {
        // Check if a portable object is inside the collider
        if(objectsToTeleport.Count != 0)
        {
            foreach(Transform currentTeleportable in objectsToTeleport)
            {
                // Relative position of current teleportable
                Vector3 localTeleportablePosition = myTransform.InverseTransformPoint(currentTeleportable.position);

                // If current teleportable is behind the teleporter
                if(localTeleportablePosition.z < 0)
                {
                    // Invert X and Z to match other portal after rotation
                    localTeleportablePosition.x = -localTeleportablePosition.x;
                    localTeleportablePosition.z = -localTeleportablePosition.z;

                    // Forward direction of current teleportable
                    Vector3 localTeleportableForward = myPortal.InverseTransformDirection(currentTeleportable.forward);

                    // Set position and rotation of current teleportable
                    currentTeleportable.position = reciever.TransformPoint(localTeleportablePosition);
                    currentTeleportable.rotation = Quaternion.LookRotation(otherPortal.TransformDirection(localTeleportableForward), otherPortal.up);
                }
            }
            objectsToTeleport.Clear();
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(myTeleportables.Contains(other.tag))
        {
            objectsToTeleport.Add(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(myTeleportables.Contains(other.tag))
        {
            objectsToTeleport.Remove(other.transform);
        }
    }
}
