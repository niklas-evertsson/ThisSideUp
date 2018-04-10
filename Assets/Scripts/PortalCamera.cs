using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PortalCamera : MonoBehaviour
{
    public Material cameraMaterial;
    public Shader cutoutShader;
    public Renderer targetPlane;
    public Transform playerCamera;
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
        // Give the camera the texture as target
        myCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        targetPlane.material.mainTexture = myCamera.targetTexture;

        myTransform = GetComponent<Transform>();
    }

	void Update()
    {
        // Match player position and rotation relative to the other portal
        myTransform.localPosition = otherPortal.InverseTransformPoint(playerCamera.position);
        myTransform.localRotation = Quaternion.LookRotation(otherPortal.InverseTransformDirection(playerCamera.forward));
	}
}
