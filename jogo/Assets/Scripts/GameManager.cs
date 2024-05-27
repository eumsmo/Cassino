using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public Controls controls;

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
