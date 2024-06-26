using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Porta : MonoBehaviour {
    public Tile ladoA, ladoB;
    public Direction direcaoAB, direcaoBA;
    public bool interactAtA = true, interactAtB = true;
    bool aberta = false;
    Vector3 targetRotation;
    bool transitioning = false;

    public float speed = 1.0f;

    void Start() {
        if (interactAtA) {
            TileInteragivel tileInteragivelA = ladoA.GetComponent<TileInteragivel>();
            tileInteragivelA.onInteract += Toggle;
        }

        if (interactAtB) {
            TileInteragivel tileInteragivelB = ladoB.GetComponent<TileInteragivel>();
            tileInteragivelB.onInteract += Toggle;
        }
    }

    void FixedUpdate() {
        if (!transitioning) return;

        //Smoothly rotate towards the target point.
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(targetRotation), Time.deltaTime * speed);

        if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(targetRotation)) < 1) {
            transform.localRotation = Quaternion.Euler(targetRotation);
            transitioning = false;
        }
    }

    public void Abrir() {
        if (aberta) return;

        aberta = true;
        ladoA.connections.Add(new Connection(ladoB, ConnectionType.Floor, direcaoAB));
        ladoB.connections.Add(new Connection(ladoA, ConnectionType.Floor, direcaoBA));

        targetRotation = new Vector3(0, 80, 0);
        transitioning = true;
    }

    public void Fechar() {
        if (!aberta) return;

        aberta = false;
        ladoA.connections.RemoveAll(c => c.tile == ladoB);
        ladoB.connections.RemoveAll(c => c.tile == ladoA);

        targetRotation = new Vector3(0, 0, 0);
        transitioning = true;
    }

    public void Toggle() {
        if (aberta) {
            Fechar();
        } else {
            Abrir();
        }
    }

    public void FecharInstant() {
        if (!aberta) return;

        aberta = false;
        ladoA.connections.RemoveAll(c => c.tile == ladoB);
        ladoB.connections.RemoveAll(c => c.tile == ladoA);

        targetRotation = new Vector3(0, 0, 0);
        transitioning = false;

        transform.localRotation = Quaternion.Euler(targetRotation);
    }
}
