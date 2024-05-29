using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller7Erros : MonoBehaviour {
    public int maxRodadas = 7;
    int rodadaAtual = 0;

    public Text portaNumeroTxt;
    Tile startTile;
    Direction startDirection;

    public Tile proximaRodadaTile, sairTile;
    public Battle_Porta porta;

    void Start() {
        startTile = BattleManager.instance.player.GetComponent<PlayerBattle>().currentTile;
        startDirection = BattleManager.instance.player.GetComponent<FPPlayer>().facingDirection;

        TileInteragivel proximoInteract = proximaRodadaTile.GetComponent<TileInteragivel>();
        proximoInteract.onInteract += Proximo;

        TileInteragivel sairInteract = sairTile.GetComponent<TileInteragivel>();
        sairInteract.onInteract += Sair;

        NovaRodada();
    }

    void NovaRodada() {
        rodadaAtual++;

        if (rodadaAtual > maxRodadas) {
            Vitoria();
            return;
        }

        portaNumeroTxt.text = "Nº " + rodadaAtual.ToString();
        FPPlayer player = BattleManager.instance.player.GetComponent<FPPlayer>();
        player.Teleport(startTile, startDirection);
        porta.FecharInstant();
    }

    void Vitoria() {
        Debug.Log("Vitoria");
    }

    void Derrota() {
        Debug.Log("Derrota");
    }

    public void Sair() {
        Debug.Log("Sair");
    }

    public void Proximo() {
        if (CheckRodadaOk()) {
            NovaRodada();
        } else {
            Derrota();
        }
    }

    public bool CheckRodadaOk() {
        return true;
    }
}
