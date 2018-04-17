using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform myPortal;
    public Transform otherPortal;
    public Transform player;
    public Transform reciever;

    private const float cooldown = 0.25f;
    private Transform myTransform;

    void Start()
    {
        myTransform = GetComponent<Transform>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if other is a Teleportable
        Teleportable teleportable = other.GetComponent<Teleportable>();
        if(teleportable != null)
        {
            if(!teleportable.IsTeleporting())
            {
                // Relative position of current teleportable
                Vector3 localTeleportablePosition = myTransform.InverseTransformPoint(other.transform.position);

                // Invert X to match other portal after rotation
                localTeleportablePosition.x = -localTeleportablePosition.x;

                // Forward direction of current teleportable
                Vector3 localTeleportableForward = myPortal.InverseTransformDirection(other.transform.forward);

                // Set position and rotation of current teleportable
                other.transform.position = reciever.TransformPoint(localTeleportablePosition);
                other.transform.rotation = Quaternion.LookRotation(otherPortal.TransformDirection(localTeleportableForward), otherPortal.up);

                if(other.CompareTag("Player"))
                {
                    PlayerController playerController = other.GetComponent<PlayerController>();
                    if(playerController != null)
                    {
                        // Align player to gravity after teleportation
                        playerController.gravityUp = reciever.up;
                    }
                }

                if(other.CompareTag("Projectile"))
                {
                    Projectile projectile = other.GetComponent<Projectile>();
                    if(projectile != null)
                    {
                        // Clear the trail renderer of the projectile after teleportation
                        projectile.ClearTrail();
                    }
                }
            }
        }
    }
}
