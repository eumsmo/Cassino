using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {
    public float speed;
    public PathPoint from, to;
    public float stopDistance = 0.1f;

    public void SetPath(PathPoint from, PathPoint to) {
        this.from = from;
        this.to = to;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (from == null || to == null) return;

        Vector3 dir = to.transform.position - from.transform.position;
        transform.rotation = from.transform.rotation;

        if (dir.magnitude > 1)
            transform.position += dir.normalized * speed * Time.fixedDeltaTime;
        
        if (Vector3.Distance(to.transform.position,transform.position) < stopDistance) {
            from.OnExit(this);
            to.OnEnter(this);

            from = to;
            to = null;
            transform.position = from.transform.position;
            transform.rotation = from.transform.rotation;
        }

        if (to != null && to.matchRotation) {
            if (to.transitionRotation) {
                transform.rotation = Quaternion.Slerp(transform.rotation, to.transform.rotation, Time.fixedDeltaTime);
            } else {
                transform.rotation = to.transform.rotation;
            }
        }
    }
}
