using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carta : MonoBehaviour {
    public static string[] valores = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public static string[] naipes = { "Paus", "Ouros", "Copas", "Espadas" };
    public string valor;
    public string naipe;

    public Text valorText1, valorText2;

    void Start() {
        valor = valores[Random.Range(0, valores.Length)];
        naipe = naipes[Random.Range(0, naipes.Length)];

        valorText1.text = valor;
        valorText2.text = valor;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            BattleUI.instance.TakeDamage(1);
        }
    }

    public static int ValorDaCarta(string valor) {
        for (int i = 0; i < valores.Length; i++) {
            if (valores[i] == valor) {
                return i+1;
            }
        }
        return -1;
    }
}
