using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogoController : MonoBehaviour {
    public Text texto;

    public System.Action OnDialogoEnd;

    Fala[] falas;
    int index = 0;

    void Start() {
        texto.text = "";
    }

    public void StartDialogo(Fala[] falas) {
        Debug.Log("Iniciando dialogo");

        this.falas = falas;
        index = 0;

        GameManager.instance.controls.UI.Interact.performed += ctx => Proximo();
        GameManager.instance.controls.UI.Enable();
        GameManager.instance.controls.Game.Disable();

        ShowFala();
    }

    void ShowFala() {
        if (falas == null) return;
        if (index >= falas.Length) {
            HandleDialogoEnd();
            return;
        }

        Fala fala = falas[index];
        texto.text = fala.texto;
        index++;
    }

    void Proximo() {
        ShowFala();
    }

    void HandleDialogoEnd() {
        OnDialogoEnd?.Invoke();

        falas = null;
        index = 0;

        texto.text = "";

        GameManager.instance.controls.UI.Interact.performed -= ctx => Proximo();
        GameManager.instance.controls.UI.Disable();
        GameManager.instance.controls.Game.Enable();
    }
}
