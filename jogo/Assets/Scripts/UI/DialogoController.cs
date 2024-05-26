using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogoController : MonoBehaviour {
    public Text texto;

    public System.Action OnDialogoEnd;

    Fala[] falas;
    int index = 0;

    public void StartDialogo(Fala[] falas) {
        this.falas = falas;
        index = 0;
        ShowFala();
    }

    public void ShowFala() {
        if (index >= falas.Length) {
            OnDialogoEnd?.Invoke();
            return;
        }

        Fala fala = falas[index];
        texto.text = fala.texto;
        index++;
    }
}
