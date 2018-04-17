using UnityEngine;

// Members of this class can use portals
public class Teleportable : MonoBehaviour
{
    private const float cooldown = 0.25f;
    private float nextTeleport;

    public bool IsTeleporting()
    {
        // Prevents flickering between portals
        if(Time.time > nextTeleport)
        {
            nextTeleport = Time.time + cooldown;
            return false;
        }

        return true;
    }
}
