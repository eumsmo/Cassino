using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carta : MonoBehaviour {
    public static string[] valores = { "Ás", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public static string[] naipes = { "Paus", "Ouros", "Copas", "Espadas" };
    public string valor;
    public string naipe;

    void Start() {
        valor = valores[Random.Range(0, valores.Length)];
        naipe = naipes[Random.Range(0, naipes.Length)];
    }
}
