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

    [Header("Chance de erro")]
    public float[] chances;
    ErroController erroController;

    void Start() {
        startTile = BattleManager.instance.player.GetComponent<PlayerBattle>().currentTile;
        startDirection = BattleManager.instance.player.GetComponent<FPPlayer>().facingDirection;

        TileInteragivel proximoInteract = proximaRodadaTile.GetComponent<TileInteragivel>();
        proximoInteract.onInteract += Proximo;

        TileInteragivel sairInteract = sairTile.GetComponent<TileInteragivel>();
        sairInteract.onInteract += Sair;

        erroController = GetComponent<ErroController>();

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
        erroController.ClearErro();

        TentarGerarErro(rodadaAtual);
    }

    void TentarGerarErro(int rodada) {
        float chance = chances[rodada];
        float rand = Random.Range(0, 1.0f);

        Debug.Log("chance: " + chance + " rand: " + rand + " res: " + (rand < chance));
        if (rand < chance) {
            
            erroController.GerarErro();
        }
    }

    void Vitoria() {
        LevelController.instance.SetVariable("$return_spot", "vip");
        LevelController.instance.ChangeLevel("Cena");
    }

    void Derrota() {
        LevelController.instance.SetVariable("$return_spot", "quartos");
        LevelController.instance.ChangeLevel("Cena");
    }

    public void Sair() {
        LevelController.instance.SetVariable("$return_spot", "quartos");
        LevelController.instance.ChangeLevel("Cena");
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
