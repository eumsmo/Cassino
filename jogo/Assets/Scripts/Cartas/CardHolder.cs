using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathPoint))]
public class CardHolder : MonoBehaviour {
    public List<string> cartas = new List<string>();

    // Mensagem enviada pelo PathPoint
    public void OnPathEnter(PathFollower follower) {
        Carta carta = follower.GetComponent<Carta>();
        if (carta == null) return;

        string valor = carta.valor;
        cartas.Add(valor);

        Destroy(follower.gameObject);
    }
}
