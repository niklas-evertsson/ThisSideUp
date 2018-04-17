using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PortalCamera : MonoBehaviour
{
    public Renderer targetRenderer;
    public Transform playerCamera;
    public Transform otherPortal;

    private Camera myCamera;
    private Transform myTransform;
    private Transform planeTransform;

    void Start()
    {
        myCamera = GetComponent<Camera>();

        // Release any assigned render texture
        if(myCamera.targetTexture != null)
        {
            myCamera.targetTexture.Release();
        }
        // Give the camera the texture as target
        myCamera.targetTexture = new RenderTexture(Screen.width / 2, Screen.height, 24);
        targetRenderer.material.mainTexture = myCamera.targetTexture;

        myTransform = GetComponent<Transform>();
        planeTransform = targetRenderer.GetComponent<Transform>();
    }

	void Update()
    {
        Vector3 planeLocalForward = planeTransform.InverseTransformDirection(planeTransform.forward);
        Vector3 playerLocalPositionToPlane = planeTransform.InverseTransformPoint(playerCamera.position);

        bool planeInView = targetRenderer.isVisible && Vector3.Dot(planeLocalForward, playerLocalPositionToPlane.normalized) < 0;
        myCamera.enabled = planeInView;

        if(planeInView)
        {
            // Match player position and rotation relative to the other portal
            myTransform.localPosition = otherPortal.InverseTransformPoint(playerCamera.position);
            myTransform.localRotation = Quaternion.LookRotation(otherPortal.InverseTransformDirection(playerCamera.forward));
        }
	}
}
