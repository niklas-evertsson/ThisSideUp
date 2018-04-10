using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportable : MonoBehaviour
{
    private const float cooldown = 0.25f;
    private float nextTeleport;

    public bool IsTeleporting()
    {
        if(Time.time > nextTeleport)
        {
            nextTeleport = Time.time + cooldown;
            return false;
        }

        return true;
    }
}
