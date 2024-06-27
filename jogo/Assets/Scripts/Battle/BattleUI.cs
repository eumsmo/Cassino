using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour {
    public static BattleUI instance;
    
    public int bossHealth = 100;
    int currentBossHealth = 100;
    public int playerHealth = 5;
    int currentPlayerHealth = 5;

    public int defesaBoss = 18;

    public Text bossHealthSlider;
    public Text playerHealthSlider;
    public Text defesaBossTxt;

    public Transform player, inimigo;
    public GameObject missel;
    public TileInteragivel[] tiles;

    void Start() {
        instance = this;
        UpdateBossHealth();
        UpdatePlayerHealth();

        // Roda nova defesa a cada 5 segundos
        InvokeRepeating("NovaDefesa", 0, 5);

        foreach (TileInteragivel tile in tiles) {
            tile.onInteract += LancarMissel;
        }
    }

    public void UpdateBossHealth() {
        bossHealthSlider.text = "Boss: " + currentBossHealth + "/" + bossHealth;
    }

    public void UpdatePlayerHealth() {
        playerHealthSlider.text = "Player: " + currentPlayerHealth + "/" + playerHealth;
    }

    public void TakeDamage(int damage) {
        currentPlayerHealth -= damage;
        UpdatePlayerHealth();

        if (currentPlayerHealth <= 0) {
            Voltar();
        }
    }

    public void BossTakeDamage(int damage) {
        if (damage < defesaBoss) return;

        currentBossHealth -= damage;
        UpdateBossHealth();

        if (currentBossHealth <= 0) {
            Voltar();
        }
    }

    void Voltar() {
        LevelController.instance.SetVariable("$return_spot", "boss");
        LevelController.instance.SetVariable("derrotou_chefe", true);
        LevelController.instance.ChangeLevel("Cena");
    }

    public void NovaDefesa() {
        defesaBoss = Random.Range(5, 17);
        defesaBossTxt.text = "Mão do boss: " + defesaBoss;
    }

    public void LancarMissel() {
        int dano = TileInteragivel.ultimoTileInteragivel.GetComponentInChildren<CardHolder>().GetValor();
        TileInteragivel.ultimoTileInteragivel.GetComponentInChildren<CardHolder>().Clear();

        GameObject novoMissel = Instantiate(missel, player.position, Quaternion.identity);
        novoMissel.GetComponent<Missel>().target = inimigo;
        novoMissel.GetComponent<Missel>().dano = dano;
    }
}
