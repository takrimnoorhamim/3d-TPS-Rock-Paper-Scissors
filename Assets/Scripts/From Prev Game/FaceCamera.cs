using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera mainCamera;



    void Update()
    {
        // Check if we have a valid camera reference
        if (mainCamera != null)
        {
            // Make the canvas look at the camera
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }
    }
}