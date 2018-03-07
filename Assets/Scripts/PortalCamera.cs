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
        // Match player position and rotation relative to the other portal
        myTransform.localPosition = otherPortal.InverseTransformPoint(playerCamera.position);
        myTransform.localRotation = Quaternion.LookRotation(otherPortal.InverseTransformDirection(playerCamera.forward));
	}
}
