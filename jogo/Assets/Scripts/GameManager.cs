using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class SpawnSpot {
    public string name;
    public Transform spot;
}

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public Controls controls;

    public List<SpawnSpot> spawnSpots;
    public GameObject player;

    public string defaultSpawn = "inicio";

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        controls = new Controls();
    }

    void Start() {

        controls.Game.Enable();
        Contexto contexto = Contexto.Global; // Contexto.LastLevel;

        string spawnSpot = LevelController.instance.ContainsVariable("$return_spot", contexto) ? (string)LevelController.instance.GetVariable("$return_spot", contexto) : defaultSpawn;
        Debug.Log("Spawn spot: " + spawnSpot);

        foreach (SpawnSpot spot in spawnSpots) {
            if (spot.name == spawnSpot) {
                Debug.Log("Spawning at " + spot.name);
                player.GetComponent<PlayerMovement>().Teleport(spot.spot.position);
            }
        }
    }

    // Estados
    public enum Estado { Jogando, Dialogo, Cutscene };
    Estado _estadoAtual = Estado.Jogando;

    public Estado estadoAtual {
        get { return _estadoAtual; }
        set {
            switch (value) {
                case Estado.Jogando:
                    SetJogando();
                    break;
                case Estado.Dialogo:
                    SetDialogando();
                    break;
                case Estado.Cutscene:
                    SetCutscene();
                    break;
            }
        }
    }

    public void SetJogando() {
        _estadoAtual = Estado.Jogando;

        controls.Game.Enable();
        controls.UI.Disable();
    }

    public void SetDialogando() {
        _estadoAtual = Estado.Dialogo;

        controls.Game.Disable();
        controls.UI.Enable();
    }

    public void SetCutscene() {
        _estadoAtual = Estado.Cutscene;

        controls.Game.Disable();
        controls.UI.Disable();
    }
}
