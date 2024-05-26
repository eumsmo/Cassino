using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interagivel : MonoBehaviour {
    LayerMask playerLayer;
    public float raio = 1.0f;
    public Vector3 offset;

    void Start() {
        playerLayer = LayerMask.GetMask("Player");
    }

    void FixedUpdate() {
        // Check if the player is in range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + offset, raio, playerLayer);

        if (hitColliders.Length == 0) return;
        
        // Check if the player is pressing the interact button
        if (Input.GetKeyDown(KeyCode.E)) {
            Interact();
        }
    }

    public virtual void Interact() {
        Debug.Log("Interagindo com " + gameObject.name);
    }

    // gizmo
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + offset, raio);
    }
}
