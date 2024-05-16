using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public Transform follow;
    public Vector3 offset;

    public float smoothSpeed = 0.125f;

    public bool useFowardAsOffset = false;
    public float distanceOfFoward = 10f;
 
    // Update is called once per frame
    void FixedUpdate() {
        Vector3 targetPosition = follow.position + offset;

        if (useFowardAsOffset) {
            targetPosition += follow.forward * distanceOfFoward;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition; 
    }
}
