using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PortalCamera : MonoBehaviour
{
    public Material cameraMaterial;
    public Transform playerCamera;
    public Transform portal;
    public Transform otherPortal;

    private Camera myCamera;
    private Transform myTransform;

    void Start()
    {
        myCamera = GetComponent<Camera>();

        // Release any assigned render texture
        if(myCamera.targetTexture != null)
        {
            myCamera.targetTexture.Release();
        }
        // Create a new render texture based on the screen size and assign it
        myCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMaterial.mainTexture = myCamera.targetTexture;

        myTransform = GetComponent<Transform>();
    }

	void Update()
    {
        // Get the angular difference between the portals as a rotation
        float portalAngleDifference = Vector3.SignedAngle(portal.forward, otherPortal.forward, Vector3.up);
        Quaternion portalRotationDifference = Quaternion.AngleAxis(-portalAngleDifference, Vector3.up);

        // Position this camera based on the players relative position to the other portal
        Vector3 playerOffsetFromPortal = portalRotationDifference * (playerCamera.position - otherPortal.position);
        myTransform.position = portal.position + playerOffsetFromPortal;

        // Match player look rotation
        Vector3 newCameraDirection = portalRotationDifference * playerCamera.forward;
        myTransform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
	}
}
