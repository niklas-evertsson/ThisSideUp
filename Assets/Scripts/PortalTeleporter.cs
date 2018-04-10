using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform myPortal;
    public Transform otherPortal;
    public Transform player;
    public PortalTeleporter reciever;

    private const float cooldown = 0.25f;
    private float nextTeleport;
    private List<string> myTeleportables;
    private Transform myTransform;
    private Transform recieverTransform;

    void Start()
    {
        myTeleportables = FindObjectOfType<GameManager>().teleportables;
        myTransform = GetComponent<Transform>();
        recieverTransform = reciever.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if(myTeleportables.Contains(other.tag) && Time.time > Mathf.Max(nextTeleport, reciever.nextTeleport))
        {
            nextTeleport = Time.time + cooldown;

            // Relative position of current teleportable
            Vector3 localTeleportablePosition = myTransform.InverseTransformPoint(other.transform.position);

            // Invert X to match other portal after rotation
            localTeleportablePosition.x = -localTeleportablePosition.x;

            // Forward direction of current teleportable
            Vector3 localTeleportableForward = myPortal.InverseTransformDirection(other.transform.forward);

            // Set position and rotation of current teleportable
            other.transform.position = recieverTransform.TransformPoint(localTeleportablePosition);
            other.transform.rotation = Quaternion.LookRotation(otherPortal.TransformDirection(localTeleportableForward), otherPortal.up);

            if(other.CompareTag("Projectile"))
            {
                Projectile projectile = other.GetComponent<Projectile>();
                if(projectile != null)
                {
                    projectile.ClearTrail();
                }
            }
        }
    }
}
