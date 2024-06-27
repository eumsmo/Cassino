using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller7Erros : MonoBehaviour {
    public static Controller7Erros instance;

    public Image blackScreen;

    public void FadeInBlackScreen(float time) {
        StartCoroutine(FadeInBlackScreenRoutine(time));
    }

    public void FadeOutBlackScreen(float time) {
        StartCoroutine(FadeOutBlackScreenRoutine(time));
    }
    
    public IEnumerator FadeInBlackScreenRoutine(float time) {
        float t = 0;
        blackScreen.gameObject.SetActive(true);
        blackScreen.color = new Color(0, 0, 0, t);

        while (t < 1) {
            t += Time.deltaTime / time;
            blackScreen.color = new Color(0, 0, 0, t);
            yield return null;
        }
    }

    public IEnumerator FadeOutBlackScreenRoutine(float time) {
        float t = 1;
        blackScreen.gameObject.SetActive(true);
        blackScreen.color = new Color(0, 0, 0, t);

        while (t > 0) {
            t -= Time.deltaTime / time;
            blackScreen.color = new Color(0, 0, 0, t);
            yield return null;
        }

        blackScreen.gameObject.SetActive(false);
    }


    public int maxRodadas = 7;
    int rodadaAtual = -1;

    public Text portaNumeroTxt;
    Tile startTile;
    Direction startDirection;

    public Tile proximaRodadaTile, sairTile;
    public Battle_Porta porta;

    [Header("Chance de erro")]
    public float[] chances;
    ErroController erroController;

    void Awake() {
        instance = this;
    }

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

    public void GetInteraction(Tile tile) {
        StartCoroutine(GetInteractionRoutine(tile));
    }

    public IEnumerator GetInteractionRoutine(Tile tile) {
        Direction dir = BattleManager.instance.player.GetComponent<FPPlayer>().facingDirection;
        yield return FadeInBlackScreenRoutine(0.5f);
        erroController.TryClearAt(tile, dir);
        yield return new WaitForSeconds(0.25f);
        yield return FadeOutBlackScreenRoutine(0.5f);
    }

    void NovaRodada() {
        StartCoroutine(NovaRodadaRoutine());
    }

    IEnumerator NovaRodadaRoutine() {
        rodadaAtual++;

        if (rodadaAtual > maxRodadas) {
            Vitoria();
        } else {
            portaNumeroTxt.text = "Nº " + rodadaAtual.ToString();
            FPPlayer player = BattleManager.instance.player.GetComponent<FPPlayer>();
            player.Teleport(startTile, startDirection);
            porta.FecharInstant();

            TentarGerarErro(rodadaAtual);

            yield return FadeOutBlackScreenRoutine(0.5f);
        }
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
        StartCoroutine(ProximoRoutine());
    }

    public IEnumerator ProximoRoutine() {
        yield return FadeInBlackScreenRoutine(0.5f);
        yield return new WaitForSeconds(0.25f);

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
