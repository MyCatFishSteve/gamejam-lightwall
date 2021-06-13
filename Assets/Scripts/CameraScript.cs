using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform Camera;

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    public float cameraHeight = 12.0f;
    public float cameraDistance = 2.0f;

    //x = left, y = up, z = right, w = down;
    public Vector4 bounds;

    void Update()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = gameObject.transform.TransformPoint(new Vector3(0, cameraHeight, cameraDistance));
        targetPosition.y = cameraHeight;

        //clamp the position to be within defined bounds
        targetPosition = new Vector3(Mathf.Clamp(targetPosition.x, bounds.x, bounds.z), cameraHeight, Mathf.Clamp(targetPosition.z, bounds.w, bounds.y));

        // Smoothly move the camera towards that target position
        Camera.position = Vector3.SmoothDamp(Camera.position, targetPosition, ref velocity, smoothTime);
    }
}
