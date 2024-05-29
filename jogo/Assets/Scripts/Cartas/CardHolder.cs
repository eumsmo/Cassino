using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PathPoint))]
public class CardHolder : MonoBehaviour {
    public List<string> cartas = new List<string>();
    public Text valor;
    public GameObject danoPrefab;
    public Transform meio;

    void Start() {
        valor.text = "0";

        // Get TileInteragivel in parent object
        TileInteragivel tile = GetComponentInParent<TileInteragivel>();
        tile.onInteract += () => {
            cartas.Clear();
            valor.text = "0";
        };

    }

    // Mensagem enviada pelo PathPoint
    public void OnPathEnter(PathFollower follower) {
        Carta carta = follower.GetComponent<Carta>();
        if (carta == null) return;

        string valor = carta.valor;
        cartas.Add(valor);

        UpdateValor();

        Destroy(follower.gameObject);
    }

    public int GetValor() {
        int valor = 0;

        foreach (string carta in cartas) {
            valor += Carta.ValorDaCarta(carta);
        }

        return valor;
    
    }

    public void UpdateValor() {
        int valor = GetValor();
        this.valor.text = valor.ToString();

        if (valor > 21) {
            Damage();
        }
    }

    public void Damage() {
        cartas.Clear();
        valor.text = "0";

        GameObject dano = Instantiate(danoPrefab, meio.position, Quaternion.identity);
    }

    public void Clear() {
        cartas.Clear();
        valor.text = "0";
    }
}
