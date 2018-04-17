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
        myTransform = GetComponent<Transform>();
        planeTransform = targetRenderer.GetComponent<Transform>();

        // Release any assigned render texture
        if(myCamera.targetTexture != null)
        {
            myCamera.targetTexture.Release();
        }

        // Create new render texture based on screen size and set it as render target on camera
        myCamera.targetTexture = new RenderTexture(Screen.width / 2, Screen.height, 24);
        targetRenderer.material.mainTexture = myCamera.targetTexture;
    }

	void Update()
    {
        // Get local references
        Vector3 planeLocalForward = planeTransform.InverseTransformDirection(planeTransform.forward);
        Vector3 playerLocalPositionToPlane = planeTransform.InverseTransformPoint(playerCamera.position);

        // Check if players can see the render plane and if its facing the players
        bool planeInView = targetRenderer.isVisible && Vector3.Dot(planeLocalForward, playerLocalPositionToPlane.normalized) < 0;

        // Else turn camera off
        myCamera.enabled = planeInView;

        if(planeInView)
        {
            // Match player position and rotation relative to the other portal
            myTransform.localPosition = otherPortal.InverseTransformPoint(playerCamera.position);
            myTransform.localRotation = Quaternion.LookRotation(otherPortal.InverseTransformDirection(playerCamera.forward));
        }
	}
}
