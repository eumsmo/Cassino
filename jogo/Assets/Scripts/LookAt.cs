using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {
    public Transform target;
    public float speed = 1.0f;


    // Update is called once per frame
    void FixedUpdate() {
        if (target == null) return;

        float time = Time.deltaTime * speed;

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), time);
    }
}
