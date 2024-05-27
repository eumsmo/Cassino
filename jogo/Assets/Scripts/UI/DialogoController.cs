using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogoController : MonoBehaviour {
    public Text texto;

    public System.Action OnDialogoEnd;

    FalaType[] falas;
    int index = 0;

    void Start() {
        texto.text = "";
    }

    public void StartDialogo(FalaType[] falas) {
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

        FalaType fala = falas[index];
        ProcessarFala(fala);
        index++;
    }

    void ProcessarFala(FalaType fala) {

        switch (fala.tipo) {
            case FalaType.Tipo.Fala:
                Fala f = (Fala)fala;
                texto.text = f.texto;
                break;
            case FalaType.Tipo.Escolha:
                break;
            case FalaType.Tipo.Acao:
                FalaAcao acao = (FalaAcao)fala;
                acao.Executar();
                break;
            case FalaType.Tipo.Nada:
                break;
            case FalaType.Tipo.Conversor:
                EditorFalas editor = ((EditorFalas) fala);
                if (editor.tipoI == FalaType.Tipo.Fala) {
                    ProcessarFala(editor.fala);
                } else if (editor.tipoI == FalaType.Tipo.Acao) {
                    ProcessarFala(editor.acao);
                }
                break;
        }
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
