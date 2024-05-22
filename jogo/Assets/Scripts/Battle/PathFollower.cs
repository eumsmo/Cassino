using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {
    public float speed;
    public PathPoint from, to;

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
        
        
        if (Vector3.Distance(to.transform.position,from.transform.position) < 1f) {
            from = to;
            to = null;
            transform.position = from.transform.position;
        }

    }
}
