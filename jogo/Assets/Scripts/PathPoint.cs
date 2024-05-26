using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour {
    public List<PathPoint> paths = new List<PathPoint>();

    public bool matchRotation = false;
    public bool transitionRotation = false;

    public PathPoint GetPath(int id) {
        return paths[id];
    }

    public void OnEnter(PathFollower follower) {
        gameObject.SendMessage("OnPathEnter", follower, SendMessageOptions.DontRequireReceiver);
    }

    public void OnExit(PathFollower follower) {
        gameObject.SendMessage("OnPathExit", follower, SendMessageOptions.DontRequireReceiver);
    }


    // Gizmos 

    void OnDrawGizmos() {
        if (paths == null) return;

        for (int i = 0; i < paths.Count; i++) {
            if (paths[i] == null) continue;

            Vector3 direction = paths[i].transform.position - transform.position;
            Vector3 position = transform.position + direction / 2;

            Gizmos.color = GetGizmoColor(i);

            Gizmos.DrawLine(transform.position, position);
        }
    }

    Color GetGizmoColor(int i) {
        Color color = Color.white;
        Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta};

        if (i < colors.Length) {
            color = colors[i];
        }

        return color;
    }
}
